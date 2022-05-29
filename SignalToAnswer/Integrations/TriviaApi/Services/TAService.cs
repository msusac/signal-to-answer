using Newtonsoft.Json;
using SignalToAnswer.Entities;
using SignalToAnswer.Integrations.TriviaApi.Exceptions;
using SignalToAnswer.Integrations.TriviaApi.Mappers;
using SignalToAnswer.Integrations.TriviaApi.Models;
using SignalToAnswer.Mappers.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SignalToAnswer.Integrations.TriviaApi.Services
{
    public class TAService
    {
        private const string BaseUrl = "https://api.trivia.willfry.co.uk/questions?";

        private readonly HttpClient _httpClient;
        private readonly TARequestMapper _taRequestMapper;
        private readonly QuestionMapper _questionMapper;

        public TAService(HttpClient httpClient, TARequestMapper taRequestMapper, QuestionMapper questionMapper)
        {
            _httpClient = httpClient;
            _taRequestMapper = taRequestMapper;
            _questionMapper = questionMapper;
        }

        public async Task<List<Question>> RetrieveQuestions(Game game)
        {
            var request = _taRequestMapper.Map(game);

            var httpResponse = await _httpClient.GetAsync(BaseUrl + request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new TriviaApiServerException(httpResponse.Content.ToString());
            }

            var content = await httpResponse.Content.ReadAsStringAsync();

            return _questionMapper.Map(JsonConvert.DeserializeObject<List<TAQuestion>>(content));
        }
    }
}
