using Microsoft.AspNetCore.Identity;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Form;
using SignalToAnswer.Repositories;
using SignalToAnswer.Validation;
using System.Threading.Tasks;

namespace SignalToAnswer.Validators.Form
{
    public class LoginFormValidator
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserRepository _userRepository;
        private readonly ValidationManager _validationManager;

        public LoginFormValidator(SignInManager<User> signInManager, UserRepository userRepository, ValidationManager validationManager)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _validationManager = validationManager;
        }

        public async Task Validate(LoginForm form)
        {
            ValidateUsername(form.UserName);
            ValidatePassword(form.Password);
            _validationManager.ThrowIfHasValidationErrors();

            await ValidateUser(form.UserName, form.Password);
        }

        private void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                _validationManager.AddValidationError("username", "Username is required!");
            }
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                _validationManager.AddValidationError("password", "Password is required!");
            }
        }

        private async Task ValidateUser(string username, string password)
        {
            var user = await _userRepository.FindOneByUsernameAndRole_Name(username, RoleType.USER);

            if (user == null)
            {
                throw new Exceptions.ValidationException("Invalid username/password!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                throw new Exceptions.ValidationException("Invalid username/password!");
            }
        }
    }
}
