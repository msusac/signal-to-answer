namespace SignalToAnswer.Dtos.GameHub
{
    public class ResultInfoDto
    {
        public string Player { get; set; }

        public int Score { get; set; }

        public int? CurrentAnswerStatus { get; set; }

        public int? CurrentAnswerScore { get; set; }

        public int WinnerStatus { get; set; }

        public string Note { get; set; }

        public ResultInfoDto(string player, int score)
        {
            Player = player;
            Score = score;
        }

        public ResultInfoDto(string player, int score, int currentAnswerStatus) : this(player, score)
        {
            CurrentAnswerStatus = currentAnswerStatus;
            CurrentAnswerScore = 0;
        }

        public ResultInfoDto(string player, int score, int currentAnswerStatus, int currentAnswerScore) : this(player, score)
        {
            CurrentAnswerStatus = currentAnswerStatus;
            CurrentAnswerScore = currentAnswerScore;
        }

        public ResultInfoDto(string player, int score, int winnerStatus, string note) : this(player, score)
        {
            WinnerStatus = winnerStatus;
            Note = note;
        }
    }
}
