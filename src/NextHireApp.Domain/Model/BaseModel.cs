using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Model
{
    public class BaseModel
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiledDate { get; set; } = DateTime.Now;
    }
}
