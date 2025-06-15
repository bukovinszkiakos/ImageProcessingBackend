using ImageProcessingWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingWebApi.Services
{
    /// <summary>
    /// Service interface for processing images.
    /// </summary>
    public interface IImageProcessingService
    {
        /// <summary>
        /// Applies image processing and returns a downloadable stream.
        /// </summary>
        /// <param name="request">The input image data and desired output format.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Processed image as a FileStreamResult.</returns>
        Task<FileStreamResult> ProcessImageAsync(ImageProcessingRequest request, CancellationToken cancellationToken);
    }
}
