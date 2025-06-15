using System.ComponentModel.DataAnnotations;

namespace ImageProcessingWebApi.Models
{
    /// <summary>
    /// Request model for submitting an image for processing.
    /// </summary>
    public class ImageProcessingRequest
    {
        /// <summary>
        /// The base64-encoded image content (e.g. PNG or JPG).
        /// </summary>
        [Required]
        public string ImageBase64 { get; set; } = string.Empty;

        /// <summary>
        /// Desired output encoding format for the processed image.
        /// </summary>
        [Required]
        public EncodingType OutputEncoding { get; set; }
    }
}
