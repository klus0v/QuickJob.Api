namespace QuickJob.DataModel.Exceptions;

public class CustomException : Exception
{
    public CustomException(string message, int statusCode) : base(message) => 
        HResult = statusCode;
}
