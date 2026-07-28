namespace Catalog.Products.Features.GetProducts
{

    public record GetProductQuery(IEnumerable<ProductDto> Products) : IQuery<GetProductResult>;

    public record GetProductResult(IEnumerable<ProductDto> Product);

    public class GetProductHandler(CatalogDbContext dbcontext)
        : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var product = await dbcontext.Products.AsNoTracking().ToListAsync(cancellationToken);

            var productDto = ProjectToProductDto(product);

            return new GetProductResult(productDto);
        }

        private List<ProductDto> ProjectToProductDto(List<Product> product)
        {
            return [];
        }
    }
}
