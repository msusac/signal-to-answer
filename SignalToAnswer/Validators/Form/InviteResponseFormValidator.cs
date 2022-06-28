using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Form;
using SignalToAnswer.Repositories;
using System.Threading.Tasks;

namespace SignalToAnswer.Validators.Form
{
    public class InviteResponseFormValidator
    {
        private readonly GameRepository _gameRepository;
        private readonly GroupRepository _groupRepository;
        private readonly PlayerRepository _playerRepository;

        public InviteResponseFormValidator(GameRepository gameRepository, GroupRepository groupRepository, PlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _groupRepository = groupRepository;
            _playerRepository = playerRepository;
        }

        public async Task Validate(InviteReplyForm form, User user)
        {
            if (!form.GameId.HasValue) {
                throw new SignalToAnswerException("Game Id is required!");
            }

            if (!form.GroupId.HasValue)
            {
                throw new SignalToAnswerException("Group Id is required!");
            }

            var game = await _gameRepository.FindOneByIdAndGameStatus(form.GameId.Value, GameStatus.WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE);
            var group = await _groupRepository.FindOneById(form.GroupId.Value);

            if (game == null || group == null)
            {
                throw new SignalToAnswerException("Selected Game was cancelled or not found!");
            }

            var player = await _playerRepository.FindOneByGameIdAndUserId(game.Id.Value, user.Id);

            if (player == null || player.InviteStatus != InviteStatus.WAITING_TO_RESPOND)
            {
                throw new SignalToAnswerException("Invalid Game invitation!");
            }
        }
    }
}
