using System.Security.Claims;

namespace QuickJob.BusinessLogic.Extensions;

public static class ClaimsPrincipalExtensions
{
    private const string SubClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"; 
    
    public static string? GetId(this ClaimsPrincipal claimsPrincipal) 
        => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == SubClaim)?.Value ?? null;
    
    public static string GetName(this ClaimsPrincipal claimsPrincipal)
    {
        Console.WriteLine("___1 " + claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
        Console.WriteLine("___2 " + claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        Console.WriteLine("___3 " + claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value);
        Console.WriteLine("___4 " + claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value);
        return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }
}