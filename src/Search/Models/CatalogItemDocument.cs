﻿namespace Search.Models;

public class CatalogItemDocument
{
    public const string IndexName = "catalog-item-index";

    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string CatalogCategory { get; set; }
    public required string CatalogBrand { get; set; }
    public required string Slug { get; set; }
    public required string DetailUrl { get; set; }
}