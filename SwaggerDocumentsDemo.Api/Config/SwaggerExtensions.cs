using Microsoft.OpenApi.Models;

namespace SwaggerDocumentsDemo.Api.Config;

public static class SwaggerExtensions
{
    private static readonly Dictionary<string, SwaggerDoc> docs = new()
    {
        { "public", new ("My Weather Api - Public endpoints", (DocumentData data)=> !data.RequiresAuthorization) },
        { "private", new ("My Weather Api - Private endpoints", (DocumentData data) => data.RequiresAuthorization) },
        { "all", new ("My Weather Api - All endpoints", (DocumentData data) => true) }
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
                var documentData = description.ActionDescriptor.EndpointMetadata.OfType<DocumentData>().FirstOrDefault();
                var doc = docs[docName];
                if (documentData == null) return false;

                return doc.Filter(documentData);
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

    private record SwaggerDoc(string Title, Func<DocumentData, bool> Filter);
}
