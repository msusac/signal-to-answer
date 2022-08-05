namespace SignalToAnswer.Dtos
{
    public class WinLossRatioDto
    {
        public int Wins { get; set; }

        public int Losses { get; set; }

        public decimal WinLossRatio { get; set; }

        public WinLossRatioDto(int wins, int losses, decimal winLossRatio)
        {
            Wins = wins;
            Losses = losses;
            WinLossRatio = winLossRatio;
        }
    }
}
