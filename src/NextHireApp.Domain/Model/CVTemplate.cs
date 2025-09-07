using System;

namespace NextHireApp.Model
{
    public class CVTemplate : BaseModel
    {
        public Guid TemplateId { get; set; }
        public string TemplateCode { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Type { get; set; }
        public string? Description { get; set; }
        public string? SampleFileUrl { get; set; }
        public int CvTemplateVersion { get; set; }
    }
}
