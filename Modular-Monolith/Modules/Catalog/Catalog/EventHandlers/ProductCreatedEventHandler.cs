namespace Catalog.EventHandlers
{
    public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
        : INotificationHandler<ProductCreatedEvent>
    {
        public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Product created event received: {}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
