using NextHireApp.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextHireApp.Services
{
    public interface ICompanyService
    {
        /// <summary>
        /// API tra cứu công ty theo mã số thuế trong nội bộ
        /// </summary>
        Task<SearchCompanyDTO?> GetCompanyByTaxCode(string taxCode);

        /// <summary>
        /// API lấy danh sách công ty
        /// </summary>
        Task<List<SearchCompanyDTO>> GetListCompany(int pageSize, int pageStart);

        /// <summary>
        /// API tạo công ty
        /// </summary>
        Task<CreateCompanyDTO> CreateCompany(CreateCompanyDTO input);

        /// <summary>
        /// API InActive công ty theo mã số thuế
        /// </summary>
        Task<bool> DeleteByTaxCode(string taxCode);

        /// <summary>
        /// API Update company
        /// </summary>
        Task<CreateCompanyDTO> UpdateCompany(UpdateCompanyDTO companyDTO);

        /// <summary>
        ///  API Verify status of company
        /// </summary>
        Task<bool> VerifyStatus(string taxCode, int status);
    }
}
