using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace NextHireApp.UserCVs
{
    public class UpdateUserCvDto
    {
        public Guid CvId { get; set; }
        public string UserCode { get; set; }
        public string CvName { get; set; }
        public IRemoteStreamContent? CvFile { get; set; }
    }
}
