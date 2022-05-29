namespace SignalToAnswer.Integrations.TriviaApi.Entities
{
    public class TAQuestionDifficulty
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Param { get; set; }

        public TAQuestionDifficulty(string name, string param)
        {
            Name = name;
            Param = param;
        }
    }
}
