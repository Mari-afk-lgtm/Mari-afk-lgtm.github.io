using System;

namespace ShootingGame
{
    [Serializable]
    public class GameResult
    {
        public string Login { get; set; }
        public int Hits { get; set; }
        public int Misses { get; set; }
        public double TimeSpent { get; set; }
        public string Difficulty { get; set; }
        public bool Win { get; set; }
        public DateTime Date { get; set; }
    }
}