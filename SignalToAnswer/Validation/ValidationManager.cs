using SignalToAnswer.Exceptions;
using System.Collections.Generic;

namespace SignalToAnswer.Validation
{
    public class ValidationManager
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationManager()
        {
            ValidationErrors = new List<ValidationError>();
        }

        public void AddValidationError(string field, string message)
        {
            ValidationErrors.Add(new ValidationError(field, message));
        }

        public void ThrowIfHasValidationErrors()
        {
            if (ValidationErrors.Count > 0)
            {
                throw new ValidationException("An Validation Exception Has Occured!", ValidationErrors);
            }
        }
    }
}
