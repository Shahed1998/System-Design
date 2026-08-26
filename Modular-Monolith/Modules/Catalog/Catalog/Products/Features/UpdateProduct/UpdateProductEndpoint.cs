namespace Catalog.Products.Features.UpdateProduct
{
    public record UpdateProductRequest(ProductDto product);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command, cancellationToken);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
                .WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Update a product")
                .WithDescription("Updates an existing product with the provided details");
        }
    }
}
