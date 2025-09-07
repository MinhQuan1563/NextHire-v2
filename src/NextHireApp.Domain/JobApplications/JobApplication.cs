using NextHireApp.AppUsers;
using NextHireApp.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace NextHireApp.JobApplications
{
    public class JobApplication : FullAuditedEntity<Guid>
    {
        public string ApplicationCode { get; set; }
        public string UserCode { get; set; }
        public string JobCode { get; set; }
        public string CvFileUrl { get; set; }
        public string CoverLetterUrl { get; set; }
        public string AttachmentFileUrl { get; set; }
        public ApplicationStatus Status { get; set; } 
        public string ApplicationSource { get; set; }
        public int Version { get; set; } = 1;
        // Navigation properties
        public virtual AppUser User { get; set; }
        public virtual Job Job { get; set; }
    }
}
