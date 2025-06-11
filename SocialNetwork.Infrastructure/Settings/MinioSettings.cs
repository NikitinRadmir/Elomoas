namespace Elomoas.Infrastructure.Settings;

public class MinioSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string LogsBucketName { get; set; } = "logs";
    public string ImagesBucketName { get; set; } = "images";
    public bool UseSSL { get; set; } = false;
} 