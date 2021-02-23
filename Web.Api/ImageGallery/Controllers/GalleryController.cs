using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController
        : ControllerBase
    {
        [HttpGet(nameof(GetGalleries))]
        public async Task<IActionResult> GetGalleries(int galleryId)
        {
            return Ok();
        }

        [HttpPost(nameof(CreateGallery))]
        public async Task<IActionResult> CreateGallery()
        {
            return Ok();
        }

        [HttpPut(nameof(UpdateGallery))]
        public async Task<IActionResult> UpdateGallery(int galleryId)
        {
            return Ok();
        }

        [HttpDelete(nameof(DeleteGallery))]
        public async Task<IActionResult> DeleteGallery(int galleryId)
        {
            return Ok();
        }
    }
}
