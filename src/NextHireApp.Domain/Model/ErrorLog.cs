using System;

namespace NextHireApp.Model
{
    public class ErrorLog
    {
        public Guid LogId { get; set; }
        public string Metadata { get; set; } = default!; // JSON
        public DateTime CreateDate { get; set; }
    }
}
