namespace SignalToAnswer.Integrations.TriviaApi.Models
{
    public class TARequest
    {
        public string Limit { get; set; }   

        public string Categories { get; set; } 

        public string Difficulty { get; set; }

        public string GetParameter()
        {
            string param = Limit;

            if (!string.IsNullOrEmpty(Categories))
            {
                param = param + "&" + Categories;
            }

            if (!string.IsNullOrEmpty(Difficulty))
            {
                param = param + "&" + Difficulty;
            }

            return param;
        }
    }
}
