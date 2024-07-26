using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Dtos.Results;

public class SuccessResult : IResult
{
    public bool IsSuccess => true;
    public IEnumerable<string> Errors => [];
}