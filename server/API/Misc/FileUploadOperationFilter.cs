using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Linq;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParam = context.ApiDescription.ParameterDescriptions
            .FirstOrDefault(p => p.ParameterDescriptor.ParameterType == typeof(IFormFile));

        if (fileParam != null)
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = 
                {
                    { 
                        "multipart/form-data", 
                        new OpenApiMediaType 
                        { 
                            Schema = new OpenApiSchema 
                            { 
                                Type = "object", 
                                Properties = 
                                {
                                    { "file", new OpenApiSchema { Type = "string", Format = "binary" } }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}