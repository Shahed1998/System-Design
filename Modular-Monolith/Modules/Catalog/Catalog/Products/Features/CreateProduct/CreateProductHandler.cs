namespace Catalog.Products.Features.CreateProduct
{

    public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductHandler(CatalogDbContext dbcontext)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = CreateNewProduct(command.Product);
            dbcontext.Products.Add(product);
            await dbcontext.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }

        private Product CreateNewProduct(ProductDto product)
        {
            var newProduct = Product.Create(
                Guid.NewGuid(),
                product.Name,
                product.Category,
                product.Description,
                product.ImageFile,
                product.Price
            );

            return newProduct;
        }
    }
}
