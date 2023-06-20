using System.Text;
using MyProduct.ClientApi.Abstractions;

namespace MyProduct.ClientApi.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public async Task<Stream> DownloadFileAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Download");
                var response = await client.GetAsync("api/download");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new MemoryStream(Encoding.UTF8.GetBytes(ex.Message)));
            }
        }


        public DownloadService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

    }
}
