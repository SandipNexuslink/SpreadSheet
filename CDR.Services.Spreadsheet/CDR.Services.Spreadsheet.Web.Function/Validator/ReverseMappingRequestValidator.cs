using CDR.Services.Spreadsheet.Model;
using FluentValidation;

namespace CDR.Services.Spreadsheet.Web.Function.Validator
{
    public class ReverseMappingRequestValidator : AbstractValidator<ReverseMappingRequest>
    {
        public ReverseMappingRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id should not be empty");
            RuleFor(x => x.BookToMap).NotEmpty().WithMessage("BookToMap should not be empty");
            RuleFor(x => x.Template).NotEmpty().WithMessage("Template should not be empty");
            RuleFor(x => x.Payload).NotEmpty().WithMessage("Payload should not be empty");
            //RuleFor(x => x.Created).NotEmpty().WithMessage("Created should not be empty");
        }
    }
}
