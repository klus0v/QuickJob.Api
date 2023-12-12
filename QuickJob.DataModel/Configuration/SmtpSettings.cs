namespace QuickJob.DataModel.Configuration;

public class SmtpSettings
{
    public int Port { get; set; }
    public string Host { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    
    public string EmailFrom { get; set; }
    public TimeSpan EmailFrequency { get; set; }
    
    
    public EmailSettings InviteSettings { get; set; }
}

public class EmailSettings
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string LinkPlace { get; set; }
    public bool IsBodyHtml { get; set; }
}