using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.UserCVs
{
    public class UserCvDto
    {
        public Guid CvId { get; set; }
        public string UserCode { get; set; }
        public string CvName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDefault { get; set; }
        public int Version { get; set; }
    }
}
