using System;

namespace SignalToAnswer.Entities
{
    public class Answer
    {
        public int? Id { get; set; }

        public int GameId { get; set; }

        public int MatchId { get; set; }

        public int PlayerId { get; set; }

        public int QuestionId { get; set; }

        public string SelectedAnswer { get; set; }

        public int? SelectedAnswerIndex { get; set; }

        public bool IsCorrectAnswer { get; set; }

        public int Score { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public Game Game { get; set; }

        public Match Match { get; set; }

        public Player Player { get; set; }

        public Question Question { get; set; }
    }
}
