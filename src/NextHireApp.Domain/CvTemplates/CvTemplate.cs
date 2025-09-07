using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace NextHireApp.CvTemplates
{
    public class CvTemplate : FullAuditedEntity<Guid>
    {
        public string TemplateCode { get; set; }
        public string Name { get; set; }
        public CvTemplateType Type { get; set; } // Dùng enum
        public string Description { get; set; }
        public string SampleFileUrl { get; set; }
        public int Version { get; set; } = 1;
    }
}
