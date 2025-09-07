using Microsoft.Extensions.Configuration;
using NextHireApp.Dtos;
using NextHireApp.Model;
using NextHireApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;

namespace NextHireApp.Services
{
    [RemoteService(false)]
    public class CompanyService : NextHireAppAppService, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IConfiguration _configuration;
        private readonly IAppUserRepository _appUserRepository;

        public CompanyService(ICompanyRepository companyRepository, IConfiguration configuration,
            IAppUserRepository appUserRepository)
        {
            _companyRepository = companyRepository;
            _configuration = configuration;
            _appUserRepository = appUserRepository;
        }

        public async Task<CreateCompanyDTO> CreateCompany(CreateCompanyDTO input)
        {
            try
            {
                // Kiểm tra UserCode tồn tại chưa
                var isUserExist = await _appUserRepository.IsExists(input.UserCode);
                if (!isUserExist)
                {
                    throw new BusinessException(NextHireAppDomainErrorCodes.UserNotFound);
                }

                // Kiểm tra Industry
                if (!Enum.IsDefined(typeof(IndustryEnum), input.Industry))
                {
                    throw new BusinessException(NextHireAppDomainErrorCodes.IndustryInvalid);
                }

                // Kiểm tra mã số thuế
                var apiBase = _configuration["VietQRSearch:api"];
                var apiUrl = $"{apiBase}{input.TaxCode}";
                using var client = new HttpClient();
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException(NextHireAppDomainErrorCodes.APINotResponse)
                        .WithData("api_name", "VietQR");
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<VietQRResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result is null || result.Code != "00")
                {
                    if (result?.Code == "51")
                        throw new BusinessException(NextHireAppDomainErrorCodes.TaxCodeInvalid);
                    if (result?.Code == "52")
                        throw new BusinessException(NextHireAppDomainErrorCodes.TaxCodeNotfound);
                    throw new BusinessException(NextHireAppDomainErrorCodes.TaxCodeAuthenFailed);
                }

                var castCompany = new Company()
                {
                    CompanyId = Guid.NewGuid(),
                    TaxCode = input.TaxCode,
                    Address = result?.Data?.Address ?? "",
                    UserCode = input.UserCode,
                    Description = input.Description,
                    LogoUrl = input.LogoUrl,
                    Website = input.Website,
                    CompanyVersion = 1,
                    Industry = (IndustryEnum)input.Industry,
                    CompanyName = result?.Data?.Name ?? "",
                    Status = 2
                };
                await _companyRepository.CreateCompany(castCompany);
                return input;

            } catch (BusinessException)
            {
                throw;
            } catch (Exception)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.CreateCompanyFailed);
            }
        }

        public async Task<bool> DeleteByTaxCode(string taxCode)
        {
            return await _companyRepository.DeleteByTaxCode(taxCode);
        }

        public async Task<SearchCompanyDTO?> GetCompanyByTaxCode(string taxCode)
        {
            var res = await _companyRepository.GetCompanyByTaxCode(taxCode);
            if (res == null) return null;
            return new SearchCompanyDTO
            {
                CompanyName = res.CompanyName,
                TaxCode = res.TaxCode,
                Address = res.Address,
                Status = res.Status
            };
        }

        public async Task<List<SearchCompanyDTO>> GetListCompany(int pageSize, int pageStart)
        {
            if (pageStart < 1)
            {
                pageStart = 0;
            }
            var res = await _companyRepository.GetListCompany(pageSize, pageStart);
            return res.Select(c => new SearchCompanyDTO
            {
                CompanyName = c.CompanyName,
                TaxCode = c.TaxCode,
                Address = c.Address,
                Status = c.Status
            }).ToList();
        }

        public async Task<CreateCompanyDTO> UpdateCompany(UpdateCompanyDTO companyDTO)
        {
            try
            {
                // Kiểm tra TaxCode tồn tại chưa
                var existingCompany = await _companyRepository.GetCompanyByTaxCode(companyDTO.TaxCode);
                if (existingCompany == null)
                {
                    throw new BusinessException(NextHireAppDomainErrorCodes.CompanyNotFound);
                }

                // Kiểm tra Industry
                if (!Enum.IsDefined(typeof(IndustryEnum), companyDTO.Industry))
                {
                    throw new BusinessException(NextHireAppDomainErrorCodes.IndustryInvalid);
                }
                var companyEntity = ObjectMapper.Map<UpdateCompanyDTO, Company>(companyDTO);
                companyEntity.CompanyId = existingCompany.CompanyId;
                companyEntity.CompanyVersion = existingCompany.CompanyVersion + 1;
                companyEntity.Status = existingCompany.Status;
                companyEntity.UserCode = existingCompany.UserCode;
                companyEntity.CreateDate = existingCompany.CreateDate;
                var res = await _companyRepository.UpdateCompany(
                    companyEntity
                );
                return ObjectMapper.Map<Company, CreateCompanyDTO>(res);
            } catch (BusinessException)
            {
                throw;
            } catch (Exception ex)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UpdateCompanyFailed)
                    .WithData("err", ex.Message);
            }
        }

        public async Task<bool> VerifyStatus(string taxCode, int status)
        {
            try
            {
                // Kiểm tra tình trạng status trên api của vietqr
                if (status != 1 && status != 2)
                {
                    throw new BusinessException(NextHireAppDomainErrorCodes.StatusInvalid);
                }

                if (status == 2)
                {
                    var apiBase = _configuration["VietQRSearch:api"];
                    var apiUrl = $"{apiBase}{taxCode}";
                    using var client = new HttpClient();
                    var response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new BusinessException(NextHireAppDomainErrorCodes.APINotResponse)
                            .WithData("api_name", "VietQR");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VietQRResponseDTO>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result is null || result.Code != "00")
                    {
                        status = 1; // InActive
                    }
                    status = 2; // Active
                }
                return await _companyRepository.VerifyStatus(taxCode, status);
            }catch (BusinessException)
            {
                throw;
            }catch (Exception ex)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UpdateCompanyFailed)
                    .WithData("err", ex.Message);
            }
        }
    }
}
