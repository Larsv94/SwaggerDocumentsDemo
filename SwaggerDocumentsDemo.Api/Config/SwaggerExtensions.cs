using Microsoft.OpenApi.Models;

namespace SwaggerDocumentsDemo.Api.Config;

public static class SwaggerExtensions
{
    private static readonly Dictionary<string, SwaggerDoc> docs = new()
    {
        { "public", new ("My Weather Api - Public endpoints", new string[] { "public" }) },
        { "private", new ("My Weather Api - Private endpoints", new string[] { "private" }) },
        { "all", new ("My Weather Api - All endpoints", new string[] { "public", "private" }) }
    };

    public static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            foreach (var doc in docs)
            {
                config.SwaggerDoc(doc.Key, new OpenApiInfo { Title = doc.Value.Title });
            }
            config.DocInclusionPredicate((docName, description) =>
            {
                var tags = description.ActionDescriptor.EndpointMetadata.OfType<TagsAttribute>().FirstOrDefault();
                var doc = docs[docName];
                if (tags == null) return false;

                return doc.Tags.Any(docTag => tags.Tags.Contains(docTag));
            });
        });
    }

    public static void UseCustomSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                foreach (var doc in docs)
                {
                    config.SwaggerEndpoint($"{doc.Key}/swagger.json", doc.Value.Title.Split(" - ")[1]);
                }
            });
        }
    }

    private record SwaggerDoc(string Title, string[] Tags);
}
