namespace SignalToAnswer.Integrations.TriviaApi.Entities
{
    public class TAQuestionCategory
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Param { get; set; }

        public TAQuestionCategory(string name, string param)
        {
            Name = name;
            Param = param;
        }
    }
}
