using System;

namespace NextHireApp.JobApplications
{
    public class JobApplicationDto
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationCode { get; set; } = default!;
        public string UserCode { get; set; } = default!;
        public string JobCode { get; set; } = default!;
        public int Status { get; set; }
        public int JobApplicateVersion { get; set; }
    }
}
