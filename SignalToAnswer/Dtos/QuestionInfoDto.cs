namespace SignalToAnswer.Dtos.GameHub
{
    public class QuestionInfoDto
    {
        public int Row { get; set; }

        public int TotalRows { get; set; }

        public string Description { get; set; }

        public int Category { get; set; }

        public int Difficulty { get; set; }

        public int ScoreMultiplier { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
