using Microsoft.AspNetCore.Identity;
using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Adapters;

public class IdentityResultAdapter : IResult
{
    private readonly IdentityResult _identityResult;

    public IdentityResultAdapter(IdentityResult identityResult)
    {
        _identityResult = identityResult;
    }

    public bool IsSuccess => _identityResult.Succeeded;
    public IEnumerable<string> Errors => _identityResult.Errors.Select(e => e.Description);
}