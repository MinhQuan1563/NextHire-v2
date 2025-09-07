using System;

namespace NextHireApp.Model
{
    public class Game
    {
        public Guid GameId { get; set; }
        public string GameCode { get; set; } = default!;
        public string GameName { get; set; } = default!;
        public int Status { get; set; }
        public string? Description { get; set; }
    }
}
