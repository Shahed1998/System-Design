namespace Catalog.EventHandlers
{
    public class ProductPriceChangeEventHandler(ILogger<ProductPriceChangeEventHandler> logger)
        : INotificationHandler<ProductPriceChangeEvent>
    {
        public Task Handle(ProductPriceChangeEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Product price change event received: {}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
