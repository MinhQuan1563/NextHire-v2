using AutoMapper;
using Microsoft.AspNetCore.Http;
using NextHireApp.Dtos;
using NextHireApp.Model;
using NextHireApp.Repository;
using NextHireApp.Services;
using NextHireApp.UserCVs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace NextHireApp.Service
{
    [RemoteService(false)]
    public class UserCvService : NextHireAppAppService, IUserCvService
    {
        private readonly IUserCvRepository _repository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IMapper _mapper;
        private readonly IFileProcessingService _fileProcessingService;

        public UserCvService(
            IUserCvRepository repository,
            IAppUserRepository appUserRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService)
        {
            _repository = repository;
            _appUserRepository = appUserRepository;
            _mapper = mapper;
            _fileProcessingService = fileProcessingService;
        }

        public async Task<UserCvDto> CreateUserCvAsync(CreateUserCvDto input)
        {
            // Verify that the user exists
            var userExists = await _appUserRepository.IsExists(input.UserCode);
            if (!userExists)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserNotFound);
            }

            // Process the uploaded CV file
            if (input.CvFile == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.FileRequired);
            }

            // Convert file to base64
            string base64File = await _fileProcessingService.ProcessFileToBase64Async(input.CvFile, "CV");

            // Create a new UserCV
            var userCv = new UserCV
            {
                CvId = Guid.NewGuid(),
                UserCode = input.UserCode,
                CvName = input.CvName,
                FileCv = base64File,
                CreatedAt = DateTime.Now,
                IsDefault = input.SetAsDefault,
                Version = 1
            };

            // If setting as default, unset any existing default CVs
            if (input.SetAsDefault)
            {
                await _repository.UnsetDefaultsForUserAsync(input.UserCode);
            }

            // Save the CV
            var result = await _repository.CreateAsync(userCv);
            
            // Return DTO
            return _mapper.Map<UserCvDto>(result);
        }

        public async Task<bool> DeleteUserCvAsync(Guid cvId, string userCode)
        {
            // Get the CV
            var cv = await _repository.GetByIdAsync(cvId);
            if (cv == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.CvNotFound);
            }

            // Check if the user owns this CV
            if (cv.UserCode != userCode)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserNotAuthorized);
            }

            // Delete the CV
            var result = await _repository.DeleteAsync(cvId);

            // If the deleted CV was the default one, set another CV as default
            if (cv.IsDefault)
            {
                var userCvs = await _repository.GetByUserCodeAsync(userCode);
                if (userCvs != null && userCvs.Count > 0)
                {
                    var newestCv = userCvs[0]; // Already ordered by CreatedAt desc
                    await _repository.UnsetDefaultsForUserAsync(userCode);
                    await _repository.SetAsDefaultAsync(newestCv.CvId, userCode);
                }
            }

            return result;
        }

        public async Task<UserCvDetailDto> GetDefaultUserCvAsync(string userCode)
        {
            var cv = await _repository.GetDefaultCvByUserCodeAsync(userCode);
            if (cv == null)
            {
                return null;
            }

            return _mapper.Map<UserCvDetailDto>(cv);
        }

        public async Task<UserCvDetailDto> GetUserCvAsync(Guid cvId)
        {
            var cv = await _repository.GetByIdAsync(cvId);
            if (cv == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.CvNotFound);
            }

            return _mapper.Map<UserCvDetailDto>(cv);
        }

        public async Task<List<UserCvDto>> GetUserCvsByUserCodeAsync(string userCode)
        {
            var cvs = await _repository.GetByUserCodeAsync(userCode);
            return _mapper.Map<List<UserCvDto>>(cvs);
        }

        public async Task<UserCvDto> SetDefaultUserCvAsync(SetDefaultUserCvDto input)
        {
            // Get the CV
            var cv = await _repository.GetByIdAsync(input.CvId);
            if (cv == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.CvNotFound);
            }

            // Check if the user owns this CV
            if (cv.UserCode != input.UserCode)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserNotAuthorized);
            }

            // Unset any existing default CVs
            await _repository.UnsetDefaultsForUserAsync(input.UserCode);

            // Set this CV as default
            await _repository.SetAsDefaultAsync(input.CvId, input.UserCode);

            // Get the updated CV
            cv = await _repository.GetByIdAsync(input.CvId);
            
            return _mapper.Map<UserCvDto>(cv);
        }

        public async Task<UserCvDto> UpdateUserCvAsync(UpdateUserCvDto input)
        {
            // Get the CV
            var cv = await _repository.GetByIdAsync(input.CvId);
            if (cv == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.CvNotFound);
            }

            // Check if the user owns this CV
            if (cv.UserCode != input.UserCode)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserNotAuthorized);
            }

            // Update the name
            cv.CvName = input.CvName;
            
            // Update the file if a new one was provided
            if (input.CvFile != null)
            {
                // Convert file to base64
                string base64File = await _fileProcessingService.ProcessFileToBase64Async(input.CvFile, "CV");
                cv.FileCv = base64File;
                cv.Version += 1; // Increment version when file changes
            }
            // Update the CV
            var result = await _repository.UpdateAsync(cv);
            
            return _mapper.Map<UserCvDto>(result);
        }
    }
}