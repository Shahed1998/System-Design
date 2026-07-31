namespace Catalog.Products.Features.GetProducts
{

    public record GetProductQuery() : IQuery<GetProductResult>;

    public record GetProductResult(IEnumerable<ProductDto> Products);

    public class GetProductHandler(CatalogDbContext dbcontext)
        : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var products = await dbcontext.Products.AsNoTracking().ToListAsync(cancellationToken);

            var productDto = products.Adapt<List<ProductDto>>();

            return new GetProductResult(productDto);
        }
    }
}
