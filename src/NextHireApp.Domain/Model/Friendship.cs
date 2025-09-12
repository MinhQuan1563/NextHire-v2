using System;

namespace NextHireApp.Model
{
    public class Friendship
    {
        public string UserCode1 { get; set; } = default!;
        public string UserCode2 { get; set; } = default!;
        public int Status { get; set; }
        public int FriendshipVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiledDate { get; set; }
    }
}
