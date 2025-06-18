using ImageProcessingWebApi.Models;
using ImageProcessingWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingWebApi.Controllers
{
    /// <summary>
    /// API controller for processing images.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ImageProcessingController : ControllerBase
    {
        private readonly IImageProcessingService _service;

        /// <summary>
        /// Constructor that injects the image processing service.
        /// </summary>
        /// <param name="service">The processing service.</param>
        public ImageProcessingController(IImageProcessingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Processes a base64-encoded image using multithreaded Gaussian blur.
        /// </summary>
        /// <param name="request">Image data and desired output format.</param>
        /// <param name="cancellationToken">Async cancellation support.</param>
        /// <returns>Processed image as file stream (PNG or JPG).</returns>
        [HttpPost("image_processing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessImage([FromBody] ImageProcessingRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.ProcessImageAsync(request, cancellationToken);
                return result;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
