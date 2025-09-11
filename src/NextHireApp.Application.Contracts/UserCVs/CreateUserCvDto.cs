using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace NextHireApp.UserCVs
{
    public class CreateUserCvDto
    {
        public string UserCode { get; set; }
        public string CvName { get; set; }
        public bool SetAsDefault { get; set; }
        // For file upload
        public IRemoteStreamContent CvFile { get; set; }
    }
}
