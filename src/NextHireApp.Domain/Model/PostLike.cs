using System;

namespace NextHireApp.Model
{
    public class PostLike
    {
        public Guid LikeId { get; set; }
        public string PostCode { get; set; } = default!;
        public string UserCode { get; set; } = default!;
        public int Type { get; set; }
        public int PostLikeVersion { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
