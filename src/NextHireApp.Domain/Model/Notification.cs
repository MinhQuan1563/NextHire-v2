using System;

namespace NextHireApp.Model
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public string UserCode { get; set; } = default!;
        public int Channel { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string? Cc { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
