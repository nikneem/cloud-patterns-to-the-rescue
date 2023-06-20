using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MyProduct.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        [HttpGet]
        public IActionResult Get()
        {
            var downloadCount = 0;
            if (_cache.TryGetValue("last-download", out int? downloadCountFromCache))
            {
                downloadCount = downloadCountFromCache.HasValue ? downloadCountFromCache.Value : downloadCount;
            }

            Console.WriteLine("download count: " + downloadCount);

            downloadCount++;
            _cache.Set("last-download", downloadCount, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });

            if (downloadCount >= 5)
            {
                return StatusCode(429, "Too many requests");
            }

            return Ok($"Downloaded {downloadCount} times");
        }


        public DownloadController(IMemoryCache cache)
        {
            _cache = cache;
        }
    }
}
