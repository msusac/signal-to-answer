using SignalToAnswer.Constants;
using SignalToAnswer.Form;
using SignalToAnswer.Repositories;
using SignalToAnswer.Validation;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SignalToAnswer.Validators.Form
{
    public class RegisterFormValidator
    {
        private readonly UserRepository _userRepository;
        private readonly ValidationManager _validationManager;

        public RegisterFormValidator(UserRepository userRepository, ValidationManager validationManager)
        {
            _userRepository = userRepository;
            _validationManager = validationManager;
        }

        public async Task Validate(RegisterForm form)
        {
            await ValidateUsername(form.UserName);
            await ValidateEmail(form.Email);
            ValidatePassword(form.Password);

            _validationManager.ThrowIfHasValidationErrors();
        }

        private async Task ValidateUsername(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                if (username.Length < 6 && username.Length < 80)
                {
                    _validationManager.AddValidationError("username", "Username must be between 8 and 80 characters!");
                }

                if ((await _userRepository.FindOneByUsernameAndRole_Name(username, RoleType.USER)) != null)
                {
                    _validationManager.AddValidationError("username", "Username already exists!");
                }
            }
            else
            {
                _validationManager.AddValidationError("username", "Username is required!");
            }
        }

        private async Task ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                EmailAddressAttribute eaa = new();

                if (!eaa.IsValid(email))
                {
                    _validationManager.AddValidationError("email", "Email address is not valid!");
                }

                if ((await _userRepository.FindOneByEmailAndRole_Name(email, RoleType.USER)) != null)
                {
                    _validationManager.AddValidationError("email", "Email address already exists!");
                }
            }
            else
            {
                _validationManager.AddValidationError("email", "Email Address is required!");
            }
        }

        private void ValidatePassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (password.Length < 8 || password.Length > 60)
                {
                    _validationManager.AddValidationError("password", "Password must be between 8 and 60 characters!");
                }
            }
            else
            {
                _validationManager.AddValidationError("password", "Password is required!");
            }
        }
    }
}
