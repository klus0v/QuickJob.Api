using FS.Keycloak.RestApiClient.Model;
using QuickJob.Api.Consumer.Models;
using QuickJob.Users.Client.Constants;

namespace QuickJob.Api.Consumer.Extensions;

public static class UserRepresentationExtensions
{
    
    public static ChanelType GetNotificationChanel(this UserRepresentation user)
    {
        //todo KeycloackUserAttributes.NtfChanel
        var stringValue = user.Attributes?
            .FirstOrDefault(x => x.Key == "KeycloackUserAttributes.NtfChanel")
            .Value?.FirstOrDefault();
        return Enum.TryParse<ChanelType>(stringValue, out var enumValue) 
            ? enumValue 
            : ChanelType.Email;
    }
    
    public static string GetFio(this UserRepresentation user) =>
        user.Attributes?
            .FirstOrDefault(x => x.Key == KeycloackUserAttributes.Fio)
            .Value?.FirstOrDefault() ?? string.Empty;
}