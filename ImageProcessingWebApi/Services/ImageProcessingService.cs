using ImageProcessingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ImageProcessingWebApi.Services
{
    /// <summary>
    /// Mock implementation of the image processing service.
    /// </summary>
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<FileStreamResult> ProcessImageAsync(ImageProcessingRequest request, CancellationToken cancellationToken)
        {
            // Mock response: returns dummy image bytes (not real image yet)
            var imageBytes = Encoding.UTF8.GetBytes("Mock image content"); //Egy "Mock image content" szöveget byte-tömbbé alakítunk UTF-8 kódolással

            //Ez úgy tesz, mintha egy képet kaptunk volna, de valójában csak próbáljuk a képfájl visszaküldését

            var stream = new MemoryStream(imageBytes);
            var contentType = request.OutputEncoding == EncodingType.PNG ? "image/png" : "image/jpeg";

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = $"processed.{request.OutputEncoding.ToString().ToLower()}"
            };
        }
    }
}
