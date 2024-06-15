using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FirmTracker_Server.Utilities.Swagger
{
    public class SwaggerDateTimeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DateTime) || context.Type == typeof(DateTime?))
            {
                schema.Format = "yyyy-MM-ddTHH:mm";
                schema.Example = new OpenApiString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm"));
            }
        }
    }
}
