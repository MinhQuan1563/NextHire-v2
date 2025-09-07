using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Model
{
    public class Job : BaseModel
    {
        public Guid JobId { get; set; }
        public string JobCode { get; set; } = default!;
        public string CompanyCode { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Salary { get; set; }
        public string? Location { get; set; }
        public int JobType { get; set; }
        public DateTime Deadline { get; set; }
        public int Status { get; set; }
        public int JobVersion { get; set; }
    }
}
