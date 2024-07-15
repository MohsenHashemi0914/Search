using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Http.HttpResults;
using Search;
using Search.Infrastructure.Extensions;
using Search.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.ConfigureElasticSearch();
builder.ConfigureBroker();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/", SearchItems);

app.Run();

static async Task<Results<Ok<IReadOnlyCollection<CatalogItemDocument>>, NotFound>> SearchItems(string q, ElasticsearchClient elasticClient)
{
    var response = await elasticClient.SearchAsync<CatalogItemDocument>(s => s
        .Index(CatalogItemDocument.IndexName)
        .From(0)
        .Size(10)
        .Query(query => query.Fuzzy(x => x.Field(z => z.Description).Value(q)))
      );

    if (!response.IsValidResponse)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(response.Documents);
}