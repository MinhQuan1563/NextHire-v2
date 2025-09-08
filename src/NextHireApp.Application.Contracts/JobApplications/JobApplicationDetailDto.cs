using System;

namespace NextHireApp.JobApplications
{ 
    public class JobApplicationDetailDto : JobApplicationDto
    {
        public string? CvFile { get; set; }
        public string? CoverLetter { get; set; }
        public string? AttachmentFile { get; set; }
    }
}