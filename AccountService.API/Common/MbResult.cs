namespace AccountService.API.Common;

public class MbResult<T>
{
    public T? Result { get; private set; }
    public MbError? Error { get; private set; }
    public bool IsSuccess => Error == null;
    public bool IsFailure => !IsSuccess;

    private MbResult(T result)
    {
        Result = result;
    }

    private MbResult(MbError error)
    {
        Error = error;
    }

    public static MbResult<T> Success(T result) => new(result);
    public static MbResult<T> Failure(MbError error) => new(error);
}