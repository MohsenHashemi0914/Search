using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;

namespace Search.Infrastructure.Extensions;

public static class ElasticSearchDependencyInjection
{
    public static void ConfigureElasticSearch(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped(sp =>
        {
            var elasticConfig = sp.GetRequiredService<IOptions<AppSettings>>().Value.ElasticSearchConfiguration;

            var settings = new ElasticsearchClientSettings(new Uri(elasticConfig.Host))
                                  .CertificateFingerprint(elasticConfig.FingerPrint)
                                  .Authentication(new BasicAuthentication(elasticConfig.UserName, elasticConfig.Password));

            return new ElasticsearchClient(settings);
        });
    }
}