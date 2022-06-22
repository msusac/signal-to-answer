namespace SignalToAnswer.Dtos.GameHub
{
    public class AnswerChoiceDto
    {
        public string Answer { get; set; }

        public int Status { get; set; }

        public bool IsDisabled { get; set; }

        public AnswerChoiceDto(string answer)
        {
            Answer = answer;
            IsDisabled = false;
        }

        public AnswerChoiceDto(string answer, int status)
        {
            Answer = answer;
            Status = status;
        }
    }
}
