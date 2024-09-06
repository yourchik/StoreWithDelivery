using Store.Application.Services.Interfaces.Results;

namespace Store.Application.ModelsDto.Results;

public class FailureResult : IResult
{
    public FailureResult(IEnumerable<string> errors)
    {
        Errors = errors;
    }
    
    public bool IsSuccess => false;
    public IEnumerable<string> Errors { get; }
}