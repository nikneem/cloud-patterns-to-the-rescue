using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProduct.ClientApi.Abstractions;

namespace MyProduct.ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDownloadService _downloadService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _downloadService.DownloadFileAsync();
            return File(data, "text/plain");
        }


        public DataController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

    }
}
