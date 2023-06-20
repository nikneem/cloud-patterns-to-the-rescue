using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyProduct.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
         var downloadedByteArray = await GetOrInitializeAsync(DownloadFromGithub, "downloadedByteArray");
         return File(downloadedByteArray, "text/json");
        }

        private async Task<T> GetOrInitializeAsync<T>(Func<Task<T>> initializeFunction, string cacheKey,
            ushort timeoutInMinutes = 15)
        {
            if (_cache.TryGetValue(cacheKey, out T? cachedValue))
            {
                if (cachedValue != null)
                {
                    return cachedValue;
                }
            }

            var initializedObject = await initializeFunction();
            _cache.Set(cacheKey, initializedObject, TimeSpan.FromMinutes(timeoutInMinutes));
            return initializedObject;
        }

        private async Task<byte[]> DownloadFromGithub()
        {
            var endpointUrl = "https://gist.githubusercontent.com/nikneem/66060dad016cabd426166f282a946f80/raw/581fe051d1b84c6799eecbdca5496b69c474292c/4DotNet-WebCasts-Caching.json";
            var client = _httpClientFactory.CreateClient("Download");
            var response = await client.GetAsync(endpointUrl);
            return await response.Content.ReadAsByteArrayAsync();
        }


        public DownloadController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

    }
}
