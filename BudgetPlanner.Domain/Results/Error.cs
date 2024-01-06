namespace BudgetPlanner.Domain.Results;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static implicit operator string(Error error) => error.Code;
    public static Error Create(string code, string message) => new(code, message);
}
