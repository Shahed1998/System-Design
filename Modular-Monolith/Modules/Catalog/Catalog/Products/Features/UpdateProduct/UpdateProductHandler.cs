namespace Catalog.Products.Features.UpdateProduct
{

    public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductHandler(CatalogDbContext dbcontext)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {

            var product = await dbcontext.Products.FindAsync(new object[] { command.Product.Id }, cancellationToken);

            if (product == null)
            {
                return new UpdateProductResult(false);
            }

            UpdateProductWithNewValues(product, command.Product);

            // Implementation for updating a product
            dbcontext.Products.Update(product);
            await dbcontext.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }

        private void UpdateProductWithNewValues(Product product, ProductDto newValues)
        {
            product.Update(
                newValues.Name,
                newValues.Category,
                newValues.Description,
                newValues.ImageFile,
                newValues.Price
            );
        }
    }
}
