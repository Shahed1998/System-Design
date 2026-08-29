using FluentValidation;

namespace Catalog.Products.Features.CreateProduct
{

    public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Product name is required");
            RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Product category is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Product price must be a positive value");
            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("Product image file is required");
        }
    }

    public record CreateProductResult(Guid Id);

    public class CreateProductHandler(CatalogDbContext dbcontext, ILogger<CreateProductHandler> logger)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle
            (CreateProductCommand command,
            CancellationToken cancellationToken)
        {

            logger.LogInformation("CreateProductHandler.Handle is called with command: {Command}", command);

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
