using FluentValidation;
using Microsoft.Extensions.Localization;
using ShopSharp.Users.Application.Constants;

namespace ShopSharp.Users.Application.Commands.CreateUser;

/// <summary>
/// Represents a validator for the <see cref="CreateUserCommand" />.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandValidator" /> class.
    /// </summary>
    /// <param name="stringLocalizer">The string localizer to use for validation error messages.</param>
    public CreateUserCommandValidator(IStringLocalizer stringLocalizer)
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage(command => stringLocalizer[ValidationConstants.RequiredMessage, nameof(command.Name)])
            .MinimumLength(UserConstants.NameMinimumLength)
            .WithMessage(command => stringLocalizer[
                ValidationConstants.MinimumCharacterLengthMessage, nameof(command.Name), UserConstants.NameMinimumLength])
            .MaximumLength(UserConstants.NameMaximumLength)
            .WithMessage(command => stringLocalizer[
                ValidationConstants.MaximumCharacterLengthMessage, nameof(command.Name), UserConstants.NameMaximumLength]);

        RuleFor(command => command.EmailAddress)
            .NotEmpty()
            .WithMessage(command => stringLocalizer[ValidationConstants.RequiredMessage, nameof(command.EmailAddress)])
            .EmailAddress()
            .WithMessage(command => stringLocalizer[ValidationConstants.ValidEmailMessage, nameof(command.EmailAddress)]);

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage(command => stringLocalizer[ValidationConstants.RequiredMessage, nameof(command.Password)])
            .MinimumLength(UserConstants.PasswordMinimumLength)
            .WithMessage(command => stringLocalizer[
                ValidationConstants.MinimumCharacterLengthMessage, nameof(command.Password), UserConstants.PasswordMinimumLength])
            .MaximumLength(UserConstants.PasswordMaximumLength)
            .WithMessage(command => stringLocalizer[
                ValidationConstants.MaximumCharacterLengthMessage, nameof(command.Password), UserConstants.PasswordMaximumLength]);
    }
}
