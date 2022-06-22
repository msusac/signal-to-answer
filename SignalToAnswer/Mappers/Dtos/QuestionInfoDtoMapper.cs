using SignalToAnswer.Dtos.GameHub;
using SignalToAnswer.Entities;

namespace SignalToAnswer.Mappers.Dtos.GameHub
{
    public class QuestionInfoDtoMapper
    {
        public QuestionInfoDto Map(Question question, int totalRows)
        {
            var dto = new QuestionInfoDto
            {
                Row = question.Row,
                TotalRows = totalRows,
                Description = question.Description,
                Category = question.Category,
                Difficulty = question.Difficulty,
                ScoreMultiplier = question.ScoreMultiplier,
            };

            return dto;
        }
    }
}
