namespace Catalog.Products.Features.GetProducts
{
    public record GetProductResponse(IEnumerable<ProductDto> Products);

    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var query = new GetProductQuery();
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Retrieves a list of products.")
            .WithDescription("Retrieves a list of products with their details.");
        }
    }
   
}
