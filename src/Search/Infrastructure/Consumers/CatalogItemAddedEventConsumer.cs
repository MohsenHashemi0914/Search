using Elastic.Clients.Elasticsearch;
using MassTransit;
using Search.Models;
using SystemDesign.InternalEvents.Catalog;

namespace Search.Infrastructure.Consumers;

public class CatalogItemAddedEventConsumer(ElasticsearchClient elasticClient)
    : IConsumer<CatalogItemAddedEvent>
{
    private readonly ElasticsearchClient _elasticClient = elasticClient;

    public async Task Consume(ConsumeContext<CatalogItemAddedEvent> context)
    {
        var message = context.Message;

        if (message is null)
        {
            return;
        }

        var document = new CatalogItemDocument
        {
            Slug = message.Slug,
            Name = message.Name,
            DetailUrl = message.DetailUrl,
            Description = message.Description,
            CatalogBrand = message.CatalogBrand,
            CatalogCategory = message.CatalogCategory
        };

        var indexExistsResponse = await _elasticClient.Indices.ExistsAsync(CatalogItemDocument.IndexName);
        if (!indexExistsResponse.Exists)
        {
            await _elasticClient.Indices.CreateAsync<CatalogItemDocument>(index: CatalogItemDocument.IndexName);
        }

        _ = await _elasticClient.IndexAsync(document, index: CatalogItemDocument.IndexName);
    }
}