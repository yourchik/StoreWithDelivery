using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Implementations.Results;

public class EntityResult<T> : IResult
{
    private readonly List<string> _errors = new List<string>();

    public bool IsSuccess { get; private set; }
    
    public T? Value { get; private set; }
    
    public IEnumerable<string> Errors => _errors;
    
    
    public static EntityResult<T> Success(T value) => 
        new EntityResult<T>(value, true);
    
    public static EntityResult<T> Failure(params string[] errors)
    {
        var result = new EntityResult<T>(default, false);
        if (errors.Length > 0)
            result._errors.AddRange(errors);
        return result;
    }

    private EntityResult(T? value, bool isSuccess)
    {
        Value = value;
        IsSuccess = isSuccess;
    }
    
    public override string ToString()
    {
        return IsSuccess
            ? "Succeeded"
            : $"Failed : {string.Join(", ", Errors)}";
    }
}

