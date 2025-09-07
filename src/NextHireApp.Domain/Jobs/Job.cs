using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace NextHireApp.Jobs
{
    public class Job : FullAuditedAggregateRoot<Guid>
    {
        public string JobCode { get; set; } = default!;
        public string CompanyCode { get; set; } = default!; // Ref: Companies.CompanyCode
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Responsibilities { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string SalaryCurrency { get; set; } = "USD";
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public bool IsRemote { get; set; } = false;

        /// <summary>
        /// Full-time, Part-time, Contract, etc.
        /// </summary>
        public int JobType { get; set; }

        /// <summary>
        /// Entry, Mid, Senior, Executive
        /// </summary>
        public int ExperienceLevel { get; set; }
        public string? CategoryCode { get; set; } // Ref: JobCategories.CategoryCode
        public DateTime? Deadline { get; set; }
        public int Status { get; set; } = 1;
        public int ViewCount { get; set; } = 0;
        public int ApplicationCount { get; set; } = 0;
        public bool IsFeatured { get; set; } = false;
        public int Version { get; set; } = 1;
        public DateTime? PublishedAt { get; set; }

    }
}
