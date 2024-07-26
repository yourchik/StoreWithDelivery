namespace Store.Application.Services.Interfaces.Results;

public interface IResult
{
    bool IsSuccess { get; }
    IEnumerable<string> Errors { get; }
}