using FluentValidation;
using TransactionApp.Application.DTOs;

namespace TransactionApp.Application.Validators
{
    public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
    {
        public CreateTransactionDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.TransactionType)
                .IsInEnum().WithMessage("Invalid transaction type");
        }
    }
}