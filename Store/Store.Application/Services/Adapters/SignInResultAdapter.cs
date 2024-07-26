using Microsoft.AspNetCore.Identity;
using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Adapters;

public class SignInResultAdapter : IResult
{
    private readonly SignInResult _signInResult;

    public SignInResultAdapter(SignInResult signInResult)
    {
        _signInResult = signInResult;
    }

    public bool IsSuccess => _signInResult.Succeeded;
    public IEnumerable<string> Errors
    {
        get
        {
            return _signInResult switch
            {
                { IsLockedOut: true } => new[] { "User account is locked out." },
                { IsNotAllowed: true } => new[] { "User is not allowed." },
                { RequiresTwoFactor: true } => new[] { "Two-factor authentication is required." },
                _ => new[] { "Invalid login attempt." }
            };
        }
    }
}