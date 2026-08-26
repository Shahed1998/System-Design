namespace Catalog.Products.Features.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<ProductDto> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category), cancellationToken);
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
                .WithName("GetProductByCategory")
                .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Products by Category")
                .WithDescription("Retrieve products by their category");
        }
    }
}
