namespace QuickJob.DataModel.Api;

public class EntityResult
{
    private static readonly EntityResult successfulResult = new();

    protected EntityResult(ErrorResult errorResult = null)
    {
        ErrorResult = errorResult;
    }

    public bool IsSuccessful => ErrorResult == null;
    public ErrorResult ErrorResult { get; }

    public bool HasErrorCode(int code) => !IsSuccessful && ErrorResult.ErrorCode.Equals(code);

    public static EntityResult CreateSuccessful() => successfulResult;
    public static EntityResult CreateError(ErrorResult error) => new(error);
}

public class EntityResult<TResponse> : EntityResult
{
    private EntityResult(TResponse response = default, ErrorResult error = null)
        : base(error)
    {
        Response = response;
    }

    public TResponse Response { get; }

    public static EntityResult<TResponse> CreateSuccessful(TResponse response) => new(response);
    public new static EntityResult<TResponse> CreateError(ErrorResult error) => new(default, error);
}

public class ErrorResult
{
    public ErrorResult(string errorMessage, int errorCode)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}


