using ImageProcessingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ImageProcessingWebApi.Services
{
    /// <summary>
    /// Service that processes base64 image data into an actual image file stream.
    /// </summary>
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<FileStreamResult> ProcessImageAsync(ImageProcessingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Decode base64 string to image bytes
                byte[] imageBytes = Convert.FromBase64String(request.ImageBase64);

                // Create stream from bytes
                var stream = new MemoryStream(imageBytes);

                // Determine MIME type from output encoding
                var contentType = request.OutputEncoding == EncodingType.PNG ? "image/png" : "image/jpeg";

                return new FileStreamResult(stream, contentType)
                {
                    FileDownloadName = $"processed.{request.OutputEncoding.ToString().ToLower()}"
                };
            }
            catch (FormatException)
            {
                return new FileStreamResult(new MemoryStream(), "text/plain")
                {
                    FileDownloadName = "error.txt"
                };
            }
        }
    }
}
