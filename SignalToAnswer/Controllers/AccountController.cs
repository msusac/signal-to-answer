using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalToAnswer.Dtos;
using SignalToAnswer.Extensions;
using SignalToAnswer.Facades;
using SignalToAnswer.Form;
using System.Threading.Tasks;

namespace SignalToAnswer.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly AccountFacade _accountFacade;

        public AccountController([FromBody] AccountFacade accountFacade)
        {
            _accountFacade = accountFacade;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> Get()
        {
            return await _accountFacade.Get(User.GetUsername());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginForm form)
        {
            return await _accountFacade.Login(form);
        }

        [HttpGet("login-as-guest")]
        public async Task<ActionResult<UserDto>> LoginAsGuest()
        {
            return await _accountFacade.LoginAsGuest();
        }

        [HttpPost("register")]
        public async Task Register(RegisterForm form)
        {
            await _accountFacade.Register(form);
        }
    }
}
