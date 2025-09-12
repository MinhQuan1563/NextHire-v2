using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace NextHireApp.JobApplications
{
public class CreateJobApplicationDto
    {

        [Required(ErrorMessage = "Vui lòng nhập mã ứng tuyển")]
        public string UserCode { get; set; } = default!;
        [Required(ErrorMessage = "Vui lòng nhập mã công việc")]
        public string JobCode { get; set; } = default!;
        public string? CvFile { get; set; }
        public string? CoverLetter { get; set; }
        public string? AttachmentFile { get; set; }
        public IRemoteStreamContent? CvFileSource { get; set; }
        public IRemoteStreamContent? CoverLetterSource { get; set; }
        public IRemoteStreamContent? AttachmentFileSource { get; set; }
    }
}