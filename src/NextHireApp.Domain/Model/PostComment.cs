using System;

namespace NextHireApp.Model
{
    public class PostComment
    {
        public Guid CommentId { get; set; }
        public Guid? ParentId { get; set; }
        public string PostCode { get; set; } = default!;
        public string UserCode { get; set; } = default!;
        public string? Content { get; set; }
        public string? Attachment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
