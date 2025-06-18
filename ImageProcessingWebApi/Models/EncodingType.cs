using System.Text.Json.Serialization;

namespace ImageProcessingWebApi.Models
{
    /// <summary>
    /// Specifies the output encoding format of the processed image.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EncodingType
    {
        PNG,
        JPG
    }
}
