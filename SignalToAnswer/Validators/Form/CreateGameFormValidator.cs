using SignalToAnswer.Constants;
using SignalToAnswer.Form;
using SignalToAnswer.Integrations.TriviaApi.Repositories;
using SignalToAnswer.Repositories;
using SignalToAnswer.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Validators.Form
{
    public class CreateGameFormValidator
    {
        private readonly TAQuestionCategoryRepository _categoryRepository;
        private readonly TAQuestionDifficultyRepository _difficultyRepository;
        private readonly UserRepository _userRepository;
        private readonly ValidationManager _validationManager;

        public CreateGameFormValidator(TAQuestionCategoryRepository categoryRepository, TAQuestionDifficultyRepository difficultyRepository,
            UserRepository userRepository, ValidationManager validationManager)
        {
            _categoryRepository = categoryRepository;
            _difficultyRepository = difficultyRepository;
            _userRepository = userRepository;
            _validationManager = validationManager;
        }

        public async Task Validate(CreateGameForm form, int gameType)
        {
            ValidateLimit(form.Limit);
            await ValidateCategories(form.Categories);
            await ValidateDifficulty(form.Difficulty);
            await ValidateInviteUsers(form.InviteUsers, gameType);

            _validationManager.ThrowIfHasValidationErrors();
        }

        private void ValidateLimit(int? limit)
        {
            if (!limit.HasValue)
            {
                _validationManager.AddValidationError("limit", "Question Limit is required!");
            }
            else if (limit < 5 || limit > 20)
            {
                _validationManager.AddValidationError("limit", "Question Limit must be between 5 and 20 questions!");
            }
        }

        private async Task ValidateCategories(List<int> categories)
        {
            if (categories.Count > 0)
            {
                var taCategories = await _categoryRepository.FindAll();

                foreach (var category in categories)
                {
                    if (taCategories.FirstOrDefault(a => a.Id == category) == null)
                    {
                        _validationManager.AddValidationError("categories", "Invalid question category!");
                        break;
                    }
                }
            }
        }

        private async Task ValidateDifficulty(int? difficulty)
        {
            if (!difficulty.HasValue)
            {
                _validationManager.AddValidationError("difficulty", "Question Difficulty is required!");
            }
            else if (!difficulty.Equals(QuestionDifficulty.DEFAULT) && (await _difficultyRepository.FindOneById(difficulty.Value) == null))
            {
                _validationManager.AddValidationError("difficulty", "Invalid question difficulty!");
            }
        }

        private async Task ValidateInviteUsers(List<string> inviteUsers, int gameType)
        {
            if (gameType == GameType.PRIVATE)
            {
                if (inviteUsers == null || !inviteUsers.Any())
                {
                    _validationManager.AddValidationError("inviteUser", "Invite User is required!");
                }
                else
                {
                    inviteUsers.ForEach(async user => {
                        if ((await _userRepository.FindOneByUsernameAndGroup_IdAndRole_Name(user, GroupType.MAIN_LOBBY, RoleType.USER)) == null)
                        {
                            _validationManager.AddValidationError("inviteUser", "User not found in main lobby!");
                        }
                    });
                }
            }
        }
    }
}
