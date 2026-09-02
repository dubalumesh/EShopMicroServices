


namespace Catlog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);

    public record DeleteProductResponse(bool IsSucess);
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("Products/{Id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(Id));
                var response = new DeleteProductResponse(result.IsScuccess);
                return Results.Ok(response);

            }).WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("DeleteProduct")
                .WithSummary("DeleteProduct");
        }
    }
}
