using System;

namespace NextHireApp.Model
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string SenderCode { get; set; } = default!;
        public string ReceiverCode { get; set; } = default!;
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsRead { get; set; }
    }
}
