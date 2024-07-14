using Elastic.Clients.Elasticsearch;
using Search;
using Search.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.ConfigureElasticSearch();
builder.ConfigureBroker();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/", async (ElasticsearchClient client) =>
{
    var tweet = new Tweet
    {
        Id = 1,
        User = "Mohsen",
        PostDate = new DateTime(2024, 11, 15),
        Message = "Trying out the client, so far so good?"
    };

    var response = await client.IndexAsync(tweet, index: "my-tweet-index");

    if (response.IsValidResponse)
    {
        Console.WriteLine($"Index document with ID {response.Id} succeeded.");
    }
})
.WithOpenApi();

app.Run();