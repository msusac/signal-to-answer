using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalToAnswer.Extensions;
using SignalToAnswer.Facades.Hubs;
using SignalToAnswer.Form;
using System.Threading.Tasks;

namespace SignalToAnswer.Controllers
{
    [ApiController]
    [Route("api/game")]
    [Authorize("RequireRoleUser")]
    public class GameController : ControllerBase
    {
        private readonly GameFacade _gameFacade;

        public GameController(GameFacade gameFacade)
        {
            _gameFacade = gameFacade;
        }

        [HttpPost("create-solo")]
        public async Task CreateSoloGame([FromBody] CreateGameForm form)
        {
            await _gameFacade.CreateSoloGame(form, User.GetUsername());
        }

        [HttpPost("create-private")]
        public async Task CreatePrivateGame([FromBody] CreateGameForm form)
        {
            await _gameFacade.CreatePrivateGame(form, User.GetUsername());
        }

        [HttpPost("reply-private-game-invite")]
        public async Task ReplyToPrivateGameInvite([FromBody] InviteReplyForm form)
        {
            await _gameFacade.ReplyToPrivateGameInvite(form, User.GetUsername());
        }
    }
}
