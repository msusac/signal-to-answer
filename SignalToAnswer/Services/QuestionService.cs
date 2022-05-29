using SignalToAnswer.Attributes;
using SignalToAnswer.Entities;
using SignalToAnswer.Repositories;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _questionRepository;

        public QuestionService(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
    }
}
