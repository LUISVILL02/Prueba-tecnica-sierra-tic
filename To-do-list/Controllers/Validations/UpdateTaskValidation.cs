using FluentValidation;
using To_do_list.DTOs;

namespace To_do_list.Controllers.Validations;

public class UpdateTaskValidation: AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidation()
    {
        RuleFor(x => x.title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required")
            .MaximumLength(50);
        RuleFor(x => x.description)
            .NotEmpty().WithMessage("Description is required")
            .NotNull().WithMessage("Description is required")
            .MaximumLength(2000);

    }
}