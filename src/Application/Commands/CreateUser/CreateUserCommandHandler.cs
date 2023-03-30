using CSharpFunctionalExtensions;
using MediatR;
using ShopSharp.Users.Application.Services;
using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.Repositories;
using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Application.Commands.CreateUser;

/// <summary>
/// Represents a handler for processing the <see cref="CreateUserCommand" />.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse, CreateUserCommandError>>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler" /> class.
    /// </summary>
    /// <param name="passwordHasher">The password hasher to use for hashing user passwords.</param>
    /// <param name="userRepository">The user repository for persisting user data.</param>
    public CreateUserCommandHandler(IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the processing of a <see cref="CreateUserCommand" />.
    /// </summary>
    /// <param name="request">The command to process.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>
    /// A Task representing the result of the command processing.
    /// The task result is a <see cref="Result{T,E}" /> containing either a <see cref="CreateUserCommandResponse" /> instance,
    /// or a <see cref="CreateUserCommandError" /> enum value indicating the reason for the failure.
    /// </returns>
    public async Task<Result<CreateUserCommandResponse, CreateUserCommandError>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var emailAddressResult = EmailAddress.Create(request.EmailAddress);

        if (emailAddressResult.IsFailure)
        {
            return MapCreateEmailAddressErrorToCreateUserCommandError(emailAddressResult.Error);
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var userResult = User.Create(request.Name, emailAddressResult.Value, hashedPassword);

        if (userResult.IsFailure)
        {
            return MapCreateUserErrorToCreateUserCommandError(userResult.Error);
        }

        await _userRepository
            .SaveAsync(userResult.Value, cancellationToken)
            .ConfigureAwait(false);

        return new CreateUserCommandResponse(userResult.Value.Id);
    }

    private static CreateUserCommandError MapCreateEmailAddressErrorToCreateUserCommandError(CreateEmailAddressError error)
    {
        return error switch
        {
            CreateEmailAddressError.EmptyOrWhitespaceEmailAddress => CreateUserCommandError.EmptyOrWhitespaceEmailAddress,
            CreateEmailAddressError.InvalidEmailAddressFormat => CreateUserCommandError.InvalidEmailAddressFormat,
            CreateEmailAddressError.None => CreateUserCommandError.None,
            _ => throw new NotImplementedException()
        };
    }

    private static CreateUserCommandError MapCreateUserErrorToCreateUserCommandError(CreateUserError error)
    {
        return error switch
        {
            CreateUserError.EmptyOrWhitespaceName => CreateUserCommandError.EmptyOrWhitespaceName,
            CreateUserError.EmptyOrWhitespacePassword => CreateUserCommandError.EmptyOrWhitespacePassword,
            CreateUserError.None => CreateUserCommandError.None,
            _ => throw new NotImplementedException()
        };
    }
}
