namespace SignalToAnswer.Dtos.GameHub
{
    public class ResultInfoDto
    {
        public string Player { get; set; }

        public int Score { get; set; }

        public int? CurrentAnswerStatus { get; set; }

        public int? CurrentAnswerScore { get; set; }

        public ResultInfoDto(string player, int score)
        {
            Player = player;
            Score = score;
            CurrentAnswerStatus = 0;
            CurrentAnswerScore = 0;
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
    }
}
