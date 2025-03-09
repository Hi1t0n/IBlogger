using System.Runtime.InteropServices;
using BaseLibrary.Classes.Result;

namespace BaseLibrary.Classes;

/// <summary>
/// Результат.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; init; }
    public string Message { get; }
    public ResultType ResultType { get; init; }
    
    private Result(T value, bool isSuccess, string message, ResultType resultType)
    {
        Value = value;
        IsSuccess = isSuccess;
        Message = message;
        ResultType = ResultType;
    }

    public static Result<T?> Failed(string message, ResultType resultType)
    {
        return new Result<T?>(default, false, message, resultType);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, string.Empty, ResultType.Ok);
    }
}