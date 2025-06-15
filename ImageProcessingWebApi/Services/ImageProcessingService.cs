using ImageProcessingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ImageProcessingNative;
using System.Diagnostics;


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
                // Decode base64 string to raw image bytes
                byte[] inputBytes = Convert.FromBase64String(request.ImageBase64);

                // Pass to C++/CLI processor (mock blur for now)
                byte[] processedBytes = Processor.MockBlur(inputBytes, request.OutputEncoding == EncodingType.PNG);

                // Create memory stream from result
                var stream = new MemoryStream(processedBytes);

                // Set MIME type
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
