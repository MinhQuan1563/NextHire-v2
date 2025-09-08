using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.JobApplications
{
    public class UpdateJobApplicationStatusDto
    {
        public string ApplicationCode { get; set; } = default!;
        public int Status { get; set; }
    }
}
