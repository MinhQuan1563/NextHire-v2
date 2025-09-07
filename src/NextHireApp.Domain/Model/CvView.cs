using System;

namespace NextHireApp.Model
{
    public class CvView
    {
        public Guid ViewId { get; set; }
        public string ViewerCode { get; set; } = default!;
        public string ViewedUserCode { get; set; } = default!;
        public DateTime ViewedAt { get; set; }
    }
}
