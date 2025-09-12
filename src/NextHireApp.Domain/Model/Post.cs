using System;

namespace NextHireApp.Model
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string PostCode { get; set; } = default!;
        public string UserCode { get; set; } = default!;
        public string? Content { get; set; }
        public string? ImageUrls { get; set; } // Json
        public int PostVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiledDate { get; set; }
        public int Privacy { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
