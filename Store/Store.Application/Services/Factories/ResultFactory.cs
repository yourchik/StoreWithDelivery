using Store.Application.ModelsDto.Results;
using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Factories;

public static class ResultFactory 
{
    public static IResult CreateResult(bool succeeded, params string[] errors)
    {
        if (succeeded)
            return new SuccessResult();

        return new FailureResult(errors);
    }
    
    public static IResult CreateResult(IResult result)
    {
        if (result.IsSuccess)
            return new SuccessResult();

        return new FailureResult(result.Errors);
    }
    
    
}
