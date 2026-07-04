namespace Catalog.Products.Events
{
    public record ProductPriceChangeEvent(Product product) : IDomainEvent;
}
