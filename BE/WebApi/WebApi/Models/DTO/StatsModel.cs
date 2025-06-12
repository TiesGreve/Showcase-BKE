namespace WebApi.Models.DTO
{
    public class StatsModel
    {
        public int TotalGames { get; set; }
        public int WinCount { get; set; }
        public int TieCount {  get; set; }
        public int LossCount { get; set; }
        public int ShortestTimeInS { get; set; }
        public int AverageTimeInS { get; set; }
        public int WinStreak {  get; set; }

    }
}
