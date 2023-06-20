namespace MyProduct.ClientApi.Abstractions;

public interface IDownloadService
{
    Task<Stream> DownloadFileAsync();
}