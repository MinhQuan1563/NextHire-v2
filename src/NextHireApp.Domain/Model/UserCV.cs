using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Model
{
    public class UserCV
    {
        public Guid CvId { get; set; }
        public string UserCode { get; set; } = default!;
        public string CvName { get; set; } = default!;
        public string FileCv { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public bool IsDefault { get; set; } = false;
        public int Version { get; set; } = 1;
        public virtual AppUser? User { get; set; }
    }
}
