namespace Defender.IdentityService.Application.Configuration.Options;

public class MongoDbOption
{
    public string AppName { get; set; } = String.Empty;
    public string Environment { get; set; } = String.Empty;
    public string ConnectionString { get; set; } = String.Empty;
}
