using System;

namespace NextHireApp.Model
{
    public class GameScore
    {
        public Guid ScoreId { get; set; }
        public string GameCode { get; set; } = default!;
        public string UserCode { get; set; } = default!;
        public int Ponit { get; set; }
        public int StreakDay { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
