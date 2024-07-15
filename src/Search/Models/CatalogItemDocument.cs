namespace Search.Models;

public class CatalogItemDocument
{
    public const string IndexName = "catalog-item-index";

    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string CatalogCategory { get; init; }
    public required string CatalogBrand { get; init; }
    public required string Slug { get; init; }
    public required string DetailUrl { get; init; }
}