namespace QuickJob.Client.Models;

public class ApiResult
{
    private static ApiResult successfulResult(int code) => new()
    {
        StatusCode = code
    };

    protected ApiResult(ErrorResult errorResult = null)
    {
        ErrorResult = errorResult;
    }

    public bool IsSuccessful => ErrorResult == null;
    public int StatusCode { get; set; }
    public ErrorResult ErrorResult { get; }

    public bool HasErrorCode(int code) => !IsSuccessful && ErrorResult.Code.Equals(code);

    public static ApiResult CreateSuccessful(int code) => successfulResult(code);

    public static ApiResult CreateError(ErrorResult error) => new(error);
}

public class ApiResult<TResponse> : ApiResult
{
    private ApiResult(TResponse response = default, int? code = null, ErrorResult error = null) : base(error)
    {
        StatusCode = code ?? 0;
        Response = response;
    }

    public TResponse Response { get; }

    public static ApiResult<TResponse> CreateSuccessful(int code, TResponse response) => new(response, code);
    public new static ApiResult<TResponse> CreateError(ErrorResult error) => new(default, null, error);
}

public class ErrorResult
{
    public ErrorResult(string message, string code)
    {
        Message = message;
        Code = code;
    }

    public string Code { get; set; }
    public string Message { get; set; }
}


