namespace QuickJob.DataModel.Configuration;

public class PostgresSettings
{
    public string DbConnectionString { get; set; }
    public string Database { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Timeout { get; set; }
    public bool UseTls { get; set; }
    public string NewsCollection { get; set; }
    public string TagsCollection { get; set; }
}