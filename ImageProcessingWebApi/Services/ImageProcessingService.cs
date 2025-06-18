using ImageProcessingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using ImageProcessingNative;


namespace ImageProcessingWebApi.Services
{
    /// <summary>
    /// Service that processes base64-encoded image data using OpenCV and returns a downloadable image stream.
    /// </summary>
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<FileStreamResult> ProcessImageAsync(ImageProcessingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Check cancellation before any processing
                cancellationToken.ThrowIfCancellationRequested();

                // Decode base64 string to raw image bytes
                byte[] inputBytes = Convert.FromBase64String(request.ImageBase64);

                // Check cancellation again before calling native code
                cancellationToken.ThrowIfCancellationRequested();

                // Apply Gaussian blur via C++/CLI module (OpenCV)
                byte[] processedBytes = await Task.Run(() =>
                {
                    // Run native call in a background thread (since it's CPU-bound)
                    return Processor.ApplyGaussianBlur(inputBytes, request.OutputEncoding == EncodingType.PNG);
                }, cancellationToken); // <-- this ensures it can be cancelled during the blur if long

                cancellationToken.ThrowIfCancellationRequested();

                if (processedBytes == null || processedBytes.Length == 0)
                {
                    throw new ApplicationException("Failed to process image. The input may be invalid or corrupted.");
                }

                // Create memory stream from result
                var stream = new MemoryStream(processedBytes);

                // Set MIME type
                var contentType = request.OutputEncoding == EncodingType.PNG ? "image/png" : "image/jpeg";

                return new FileStreamResult(stream, contentType)
                {
                    FileDownloadName = $"processed.{request.OutputEncoding.ToString().ToLower()}"
                };
            }
            catch (OperationCanceledException)
            {
                // Explicitly rethrow as 400-cancelled if needed or just bubble up
                throw new OperationCanceledException("Image processing was canceled.");
            }
            catch (FormatException)
            {
                // Invalid base64 format
                throw new ArgumentException("Invalid base64 image format.");
            }
            catch (System.Reflection.TargetInvocationException ex) when (ex.InnerException is System.Exception inner)
            {
                // Forward native C++/CLI exceptions as ArgumentException for 400 response
                throw new ArgumentException(inner.Message);
            }
            catch (Exception ex)
            {
                // Catch-all for any unexpected errors
                throw new ApplicationException("Image processing failed.", ex);
            }

        }
    }
}

