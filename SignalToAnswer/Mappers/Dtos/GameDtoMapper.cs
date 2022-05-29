using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;

namespace SignalToAnswer.Mappers.Dtos
{
    public class GameDtoMapper
    {
        public GameDto Map(Game game)
        {
            var gameDto = new GameDto
            {
                Id = game.Id.Value,
                Type = game.GameType
            };

            return gameDto;
        }

        public GameDto Map(Game game, int inviteId, string invitedBy)
        {
            var gameDto = new GameDto
            {
                Id = game.Id.Value,
                Type = game.GameType,
                InviteId = inviteId,
                InvitedBy = invitedBy
            };

            return gameDto;
        }
    }
}
