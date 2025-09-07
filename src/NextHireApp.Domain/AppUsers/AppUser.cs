using System;
using Volo.Abp.Identity;

namespace NextHireApp.AppUsers
{
    public class AppUser : IdentityUser
    {
        public string UserCode { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Gender { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Skills { get; set; }

        public string? Experience { get; set; }

        public string? Education { get; set; }

        public string? PersonalProjects { get; set; }

        public string? PortfolioUrl { get; set; }

        public string? SavedJobs { get; set; }
        protected AppUser() { }
        public AppUser(Guid id, string userName, string email,string userCode,string fullName,Guid? tenantId = null) : base(id,userName, email,tenantId)
        {
            UserCode = userCode;
            FullName = fullName;
        }
    }
}