using Store.Application.Services.Interfaces.Results;

namespace Store.Application.ModelsDto.Results;

public class SuccessResult : IResult
{
    public bool IsSuccess => true;
    public IEnumerable<string> Errors => [];
}