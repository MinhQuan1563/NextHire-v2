using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.JobApplications
{
    public class CancelJobApplicationDto
    {
        [Required(ErrorMessage = "Vui lòng nhập mã ứng tuyển")]
        public string ApplicationCode { get; set; } = default!;
        [Required(ErrorMessage = "Vui lòng nhập mã người dùng")]
        public string UserCode { get; set; } = default!;
    }
}
