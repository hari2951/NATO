using FluentValidation.TestHelper;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Validators;

namespace TransactionApp.UnitTests.Validators
{
    public class CreateUserDtoValidatorTests
    {
        private readonly CreateUserDtoValidator _validator = new();

        [Fact]
        public void ValidDto_Should_Pass()
        {
            var dto = new CreateUserDto { FirstName = "Alan", LastName = "Bab", Email = "Alan@test.com" };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Empty_FirstName_Should_Fail()
        {
            var dto = new CreateUserDto { FirstName = "", LastName = "Jones", Email = "TJ@test.com" };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage("First name is required");
        }


        [Fact]
        public void Invalid_Email_Should_Fail()
        {
            var dto = new CreateUserDto { FirstName = "Charlie", LastName = "West", Email = "charliegmail.com" };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email must be valid");
        }

        [Fact]
        public void Empty_Email_Should_Fail()
        {
            var dto = new CreateUserDto { FirstName = "Jacob", LastName = "Tom", Email = "" };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email is required");
        }
    }
}
