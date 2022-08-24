using SignalToAnswer.Constants;
using SignalToAnswer.Dtos;
using SignalToAnswer.Form;
using SignalToAnswer.Mappers.Dtos;
using SignalToAnswer.Services;
using SignalToAnswer.Validators.Form;
using System.Threading.Tasks;

namespace SignalToAnswer.Facades
{
    public class AccountFacade
    {
        private readonly UserDtoMapper _userDtoMapper;
        private readonly TokenService _tokenService;
        private readonly UserService _userService;
        private readonly LoginFormValidator _loginFormValidator;
        private readonly RegisterFormValidator _registerFormValidator;

        public AccountFacade(UserDtoMapper userDtoMapper, TokenService tokenService, UserService userService,
            LoginFormValidator loginFormValidator, RegisterFormValidator registerFormValidator)
        {
            _userDtoMapper = userDtoMapper;
            _tokenService = tokenService;
            _userService = userService;
            _loginFormValidator = loginFormValidator;
            _registerFormValidator = registerFormValidator;
        }

        public async Task<UserDto> Get(string username)
        {
            var user = await _userService.GetOne(username);

            return _userDtoMapper.Map(user, null);
        }

        public async Task<UserDto> Login(LoginForm form)
        {
            await _loginFormValidator.Validate(form);

            var user = await _userService.GetOne(form.UserName, "USER");
            var token = await _tokenService.GenerateToken(user, RoleType.USER);

            return _userDtoMapper.Map(user, token);
        }

        public async Task<UserDto> LoginAsGuest()
        {
            var user = await _userService.CreateGuest();
            var token = await _tokenService.GenerateToken(user, RoleType.GUEST);

            return _userDtoMapper.Map(user, token);
        }

        public async Task Register(RegisterForm form)
        {
            await _registerFormValidator.Validate(form);

            await _userService.CreateUser(form.UserName, form.Email, form.Password);
        }
    }
}
