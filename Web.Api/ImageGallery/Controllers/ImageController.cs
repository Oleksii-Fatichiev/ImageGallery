using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController
        : ControllerBase
    {
        [HttpGet(nameof(GetImage))]
        public async Task<IActionResult> GetImage(int imageId)
        {
            return Ok();
        }

        [HttpGet(nameof(GetImages))]
        public async Task<IActionResult> GetImages(int galleryId)
        {
            return Ok();
        }

        [HttpPost(nameof(CreateImage))]
        public async Task<IActionResult> CreateImage()
        {
            return Ok();
        }

        [HttpPut(nameof(UpdateImage))]
        public async Task<IActionResult> UpdateImage(int imageId)
        {
            return Ok();
        }

        [HttpDelete(nameof(DeleteImage))]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            return Ok();
        }
    }
}
