namespace Catalog.Products.Features.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductResult>;

    public record GetProductResult(ProductDto Product);

    public class GetProductByIdHandler(CatalogDbContext dbcontext)
        : IQueryHandler<GetProductByIdQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await dbcontext.Products.AsNoTracking().SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (product is null)
            {
                throw new Exception($"Product with id {query.Id} not found.");
            }

            var productDto = product.Adapt<ProductDto>();

            return new GetProductResult(productDto);
        }
    }
}
