using System;
using NextHireApp.AppUsers;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
namespace NextHireApp.UserCvs
{
    public class UserCv : FullAuditedEntity<Guid>
    {
        public string UserCode { get; set; }
        public string CvName { get; set; }
        public string Base64Source { get; set; }
        public bool IsDefault { get; set; } = false;
        public int Version { get; set; } = 1;
        public virtual AppUser User { get; set; }
    }
}
