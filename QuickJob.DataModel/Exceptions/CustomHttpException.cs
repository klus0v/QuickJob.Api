using System.Net;

namespace QuickJob.DataModel.Exceptions;

public class CustomHttpException : Exception
{
    public HttpStatusCode CustomHttpCode { get; private set; }
    public CustomHttpError CustomError { get; private set; }
    
    public CustomHttpException(HttpStatusCode customHttpCode, CustomHttpError customError)
    {
        CustomHttpCode = customHttpCode;
        CustomError = customError;
    }
    
    public CustomHttpException(int customHttpCode, CustomHttpError customError)
    {
        CustomHttpCode = (HttpStatusCode)customHttpCode;
        CustomError = customError;
    }
}
