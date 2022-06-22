using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class Question
    {
        public int? Id { get; set; }

        public int Row { get; set; }

        public int GameId { get; set; }

        public int MatchId { get; set; }

        public int Category { get; set; }

        public int Difficulty { get; set; }

        public int ScoreMultiplier { get; set; }

        public string Description { get; set; }

        public string CorrectAnswer { get; set; }

        public int CorrectAnswerIndex { get; set; }

        public int RemainingTime { get; set; }

        public List<string> AnswerChoices { get; set; }

        public bool? IsOngoing { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public Game Game { get; set; }

        public Match Match { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
