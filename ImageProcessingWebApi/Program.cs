using ImageProcessingWebApi.Services;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace ImageProcessingWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Allow processing of large base64 image requests up to 50MB
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; 
            });

            // Add controllers and configure JSON options to serialize enums as strings.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Ensures that enums like EncodingType are serialized/deserialized as strings (e.g., "PNG")
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Register the image processing service
            builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();

            // Enable Swagger and API endpoint discovery
            builder.Services.AddEndpointsApiExplorer();

            // Configure Swagger generation and support for non-nullable reference types
            builder.Services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes();

                // Ensures enums like EncodingType appear as inline dropdowns in Swagger UI
                options.UseInlineDefinitionsForEnums(); 
            });

            var app = builder.Build();

            // Enable Swagger UI only in Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
