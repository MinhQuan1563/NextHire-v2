using System;

namespace NextHireApp.Model
{
    public class CvView
    {
        public Guid ViewId { get; set; }
        public Guid ViewedCvId { get; set; } = default!;
        public string ViewedUserCode { get; set; } = default!;
        public DateTime ViewedAt { get; set; }
        public virtual AppUser? Viewer { get; set; }
        public virtual UserCV? Cv { get; set; }
    }
}
