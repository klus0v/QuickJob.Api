namespace QuickJob.DataModel.Exceptions;

public sealed record CustomHttpError(string? Code, string? Message = null)
{
    
}
