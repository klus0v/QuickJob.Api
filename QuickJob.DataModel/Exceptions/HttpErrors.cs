namespace QuickJob.DataModel.Exceptions;

public static class HttpErrors
{
    private const string PgError = "PgError";
    private const string AWSError = "AWSError";
    private const string NotFoundError = "NotFoundError";
    private const string NoAccessError = "NoAccess";
    private const string LimitExceededError = "LimitExceeded";

    public static CustomHttpError Pg(string error) => new(PgError, $"PgError: {error}");
    public static CustomHttpError AWS(string error) => new(AWSError, $"AWSError: {error}");
    public static CustomHttpError NotFound(object itemKey) => new(NotFoundError, $"Not found item with key: '{itemKey}'");
    public static CustomHttpError NoAccess(object itemKey) => new(NoAccessError, $"No access to item with key: '{itemKey}'");
    public static CustomHttpError LimitExceeded() => new(LimitExceededError, "Workers limit exceeded");

}
