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

            // Configure Swagger to show enums as strings and map EncodingType manually
            builder.Services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes(); 
                options.UseInlineDefinitionsForEnums(); 

                // Manually define string values for the EncodingType enum
                options.MapType<Models.EncodingType>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString("PNG"),
                        new OpenApiString("JPG")
                    }
                });
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
