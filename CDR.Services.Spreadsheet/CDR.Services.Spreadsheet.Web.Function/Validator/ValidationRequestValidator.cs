using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using FluentValidation;

namespace CDR.Services.Spreadsheet.Web.Function.Validator
{
    public class ValidationRequestValidator : AbstractValidator<ValidationRequest>
    {
        public ValidationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id should not be empty");
            RuleFor(x => x.BookUnderValidation).NotEmpty().WithMessage("BookUnderValidation should not be empty");
            RuleFor(x => x.ValidatorBook).NotEmpty().WithMessage("ValidatorBook should not be empty");
            RuleFor(x => x.Created).NotEmpty().WithMessage("Created should not be empty");
        }
    }
}
