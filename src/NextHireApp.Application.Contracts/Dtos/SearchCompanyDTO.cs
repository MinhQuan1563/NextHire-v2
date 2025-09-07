using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Dtos
{
    public class SearchCompanyDTO
    {
        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
    }
}
