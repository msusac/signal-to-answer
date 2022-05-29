using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalToAnswer.Extensions;
using SignalToAnswer.Facades;
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
        public async Task<int> CreateSoloGame([FromBody] CreateGameForm form)
        {
            return await _gameFacade.CreateSoloGame(form, User.GetUsername());
        }

        [HttpPost("create-private")]
        public async Task CreatePrivateGame([FromBody] CreateGameForm form)
        {
            await _gameFacade.CreatePrivateGame(form, User.GetUsername());
        }

        [HttpPost("respond-private-game-invite")]
        public async Task RespondToPrivateGameInvite([FromBody] InviteResponseForm form)
        {
            await _gameFacade.RespondToPrivateGameInvite(form, User.GetUsername());
        }
    }
}
