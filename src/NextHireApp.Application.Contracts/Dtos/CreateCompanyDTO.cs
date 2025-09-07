using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Dtos
{
    public class CreateCompanyDTO
    {
        public string TaxCode { get; set; }
        public string UserCode { get; set; }
        public string CompanyName { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public int Industry { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
    }
}
