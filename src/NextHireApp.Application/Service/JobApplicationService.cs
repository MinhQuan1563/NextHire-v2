using AutoMapper;
using NextHireApp;
using NextHireApp.Dtos;
using NextHireApp.JobApplications;
using NextHireApp.Model;
using NextHireApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace NextHireApp.Service
{
    [RemoteService(false)]
    public class JobApplicationService : NextHireAppAppService, IJobApplicationService
    {
        private readonly IJobApplicationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IFileProcessingService _fileProcessingService;

        public JobApplicationService(IJobApplicationRepository repository, 
            IMapper mapper, 
            IAppUserRepository appUserRepository,
            IJobRepository jobRepository,
            IFileProcessingService fileProcessingService)
        {
            _repository = repository;
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _jobRepository = jobRepository;
            _fileProcessingService = fileProcessingService;
        }
        /// <summary>
        /// Người tìm việc nhấn apply vào công việc
        /// </summary>
        public async Task<JobApplicationDto> CreateJobApplicationAsync(CreateJobApplicationDto input)
        {
            var current = DateTime.Now;
            // Kiểm tra UserCode tồn tại chưa
            var isUserExist = await _appUserRepository.IsExists(input.UserCode);
            if (!isUserExist)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserNotFound);
            }
            // Kiểm tra Job tồn tại chưa && quá hạn chưa
            var job = await _jobRepository.GetJobByCodeAsync(input.JobCode);
            if (job == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.AppliedJobNotFound);
            }
            var isAlreadyApplied = await _repository.HasUserAppliedToJobAsync(input.UserCode,input.JobCode);
            if (isAlreadyApplied) {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserAlreadyApplyToJobs);
            }
            if (current > job.Deadline)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.MissedJobDeadline);
            }
            // Xử lý các file được tải lên bằng FileProcessingService
            if (input.CvFileSource != null)
            {
                input.CvFile = await _fileProcessingService.ProcessFileToBase64Async(input.CvFileSource, "CV");
            }

            if (input.CoverLetterSource != null)
            {
                input.CoverLetter = await _fileProcessingService.ProcessFileToBase64Async(input.CoverLetterSource, "CoverLetter");
            }

            if (input.AttachmentFileSource != null)
            {
                input.AttachmentFile = await _fileProcessingService.ProcessFileToBase64Async(input.AttachmentFileSource, "Attachment");
            }
            var entity = _mapper.Map<JobApplication>(input);
            entity.ApplicationCode = GenerateApplicationCode();
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<JobApplicationDto>(result);
        }

        /// <summary>
        /// Lấy thông tin hồ sơ ứng tuyển theo mã code
        /// </summary>
        public async Task<JobApplicationDto> GetJobApplicationByCodeAsync(string applicationCode)
        {
            var application = await _repository.GetByCodeAsync(applicationCode);
            if (application == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.JobApplicationNotFound);
            }
            
            return _mapper.Map<JobApplicationDto>(application);
        }
        public async Task<List<JobApplicationDto>> GetJobApplicationsByUserCodeAsync(string userCode)
        {
            var applications = await _repository.GetByUserCodeAsync(userCode);
            if (applications == null) return null;
            return _mapper.Map<List<JobApplicationDto>>(applications);
        }
        /// <summary>
        /// Lấy danh sách ứng viên đã ứng tuyển vào một công việc
        /// </summary>
        public async Task<List<JobApplicationDto>> GetJobApplicationsByJobCodeAsync(string jobCode)
        {
            var applications = await _repository.GetByJobCodeAsync(jobCode);
            if (applications == null) return null;
            return _mapper.Map<List<JobApplicationDto>>(applications);
        }

        /// <summary>
        /// Cập nhật trạng thái hồ sơ ứng tuyển (từ chối, phỏng vấn, chấp nhận...)
        /// </summary>
        public async Task<JobApplicationDto> UpdateApplicationStatusAsync(UpdateJobApplicationStatusDto input)
        {
            var application = await _repository.GetByCodeAsync(input.ApplicationCode);
            if (application == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.JobApplicationNotFound);
            }

            if (!Enum.IsDefined(typeof(ApplicationStatus), input.Status))
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.InvalidJobApplicationStatus);
            }

            application.Status = input.Status;
            var result = await _repository.UpdateAsync(application);
            return _mapper.Map<JobApplicationDto>(result);
        }

        /// <summary>
        /// Kiểm tra người dùng đã ứng tuyển vào công việc này chưa
        /// </summary>
        public async Task<bool> HasUserAppliedToJobAsync(string userCode, string jobCode)
        {
            return await _repository.HasUserAppliedToJobAsync(userCode, jobCode);
        }

        /// <summary>
        /// Cho phép ứng viên hủy đơn ứng tuyển (chỉ khi trạng thái còn là "Đang chờ xét duyệt")
        /// </summary>
        public async Task<bool> CancelJobApplicationAsync(string applicationCode, string userCode)
        {
            var application = await _repository.GetByCodeAsync(applicationCode);
            if (application == null)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.JobApplicationNotFound);
            }
            
            // Kiểm tra xem người hủy có phải là chủ hồ sơ không
            if (application.UserCode != userCode)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.UserNotAuthorized);
            }
            
            // Chỉ cho phép hủy khi đơn đang ở trạng thái "Đang chờ xét duyệt"
            if (application.Status != (int)ApplicationStatus.Applied && application.Status != (int)ApplicationStatus.Reviewed)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.CannotCancelProcessedApplication);
            }

            application.Status = (int)ApplicationStatus.Cancelled;
            await _repository.UpdateAsync(application);
            return true;
        }
        /// <summary>
        /// Tạo mã ApplicationCode ngẫu nhiên với định dạng phù hợp với giới hạn cột trong DB
        /// </summary>
        private string GenerateApplicationCode()
        {
            // Generate a short 12-character code (based on migration history showing nvarchar(12))
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Create a unique code with 8 random characters (APP + 8 chars = 11 total)
            var randomCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"APP{randomCode}";
        }
        /// <summary>
        /// Lấy thông tin chi tiết hồ sơ ứng tuyển kèm theo các file đính kèm (CV, Cover Letter, Attachment)
        /// </summary>
        public async Task<JobApplicationDetailDto> GetJobApplicationDetailByCodeAsync(string applicationCode)
        {
            var application = await _repository.GetByCodeAsync(applicationCode);
            if (application == null) return null;

            return _mapper.Map<JobApplicationDetailDto>(application);
        }
    }
}
