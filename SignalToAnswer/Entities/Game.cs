using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class Game
    {
        public int? Id { get; set; }

        public int GameType { get; set; }

        public int GameStatus { get; set; }

        public int MaxPlayerCount { get; set; }

        public int QuestionsCount { get; set; }

        public List<int> QuestionCategories { get; set; }

        public int QuestionDifficulty { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public List<Answer> Answers { get; set; }

        public List<Match> Matches { get; set; }

        public List<Player> Players { get; set; }

        public List<Question> Questions { get; set; }

        public List<Result> Result { get; set; } 
    }
}
