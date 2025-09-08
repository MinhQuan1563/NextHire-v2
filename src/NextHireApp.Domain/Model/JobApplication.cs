using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Model
{
    public class JobApplication : BaseModel
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationCode { get; set; } = default!;
        public string UserCode { get; set; } = default!;
        public string JobCode { get; set; } = default!;
        public string? CvFile { get; set; }
        public string? CoverLetter { get; set; }
        public string? AttachmentFile { get; set; }
        public int Status { get; set; }
        public int JobApplicateVersion { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual Job? Job { get; set; }
    }
}
