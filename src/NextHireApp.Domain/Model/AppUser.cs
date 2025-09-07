using System;
using Volo.Abp.Identity;

namespace NextHireApp.Model
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public required string UserCode { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? Education { get; set; }
        public string? PersonalProjects { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? SavedJobs { get; set; }
    }
}
