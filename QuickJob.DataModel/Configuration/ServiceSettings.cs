namespace QuickJob.DataModel.Configuration;

public class ServiceSettings
{
    public List<string> Origins { get; set; }
    public string FrontRegisterUrl { get; set; }
    public string UsersApiKey { get; set; }
    public string UsersApiBaseUrl { get; set; }
    public string ApiKey { get; set; }
    public string ApiBaseUrl { get; set; }
    public Templates Templates { get; set; }
}

public class Templates
{
    public string ApprovedRespondEmail { get; set; }

}