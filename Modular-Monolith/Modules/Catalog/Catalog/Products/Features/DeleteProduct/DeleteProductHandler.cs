using FluentValidation;

namespace Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required");
        }
    }

    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductHandler(CatalogDbContext dbcontext)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {

            var product = await dbcontext.Products.FindAsync(new object[] { command.ProductId }, cancellationToken);

            if (product == null)
            {
                return new DeleteProductResult(false);
            }

            // Implementation for deleting a product
            dbcontext.Products.Remove(product);
            await dbcontext.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
