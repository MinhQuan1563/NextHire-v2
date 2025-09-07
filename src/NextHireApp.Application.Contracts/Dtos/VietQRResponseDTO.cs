using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Dtos
{
    public class VietQRResponseDTO
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public VietQRBusinessData? Data { get; set; }
    }

    public class VietQRBusinessData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InternationalName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
    }
}
