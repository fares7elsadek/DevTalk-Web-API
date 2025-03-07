namespace DevTalk.Domain.Helpers;

public class BlobStorageOptions
{
    public string ConnectionString { get; set; } = default!;
    public string PhotosContainerName { get; set; } = default!;
}
