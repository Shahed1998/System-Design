using FluentValidation;

namespace Catalog.Products.Features.UpdateProduct
{

    public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

    public class  UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Product name is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Product price must be a positive value");
        }
    }

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductHandler(CatalogDbContext dbcontext)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {

            var product = await dbcontext.Products.FindAsync(new object[] { command.Product.Id }, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(command.Product.Id);
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
