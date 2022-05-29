using SignalToAnswer.Validation;
using System;
using System.Collections.Generic;

namespace SignalToAnswer.Exceptions
{
    public class ValidationException : Exception
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationException(string message = null, List<ValidationError> validationErrors = null) : base(message)
        {
            ValidationErrors = validationErrors;
        }
    }
}
