
using Basket.API.Basket.StoreBasket;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketRequest(String UserName);

    public record DeleteBasketResponse(bool IsSuccess);

    // Delete basket endpoint to delete basket with userName as parameter
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{UserName}", async (string UserName, ISender sender) =>
            {
                var command = new DeleteBasketCommand(UserName);
                var result = await sender.Send(command, CancellationToken.None);

                DeleteBasketResponse response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
             .WithName("DeleteBasket")
              .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("DeleteBasket")
                .WithSummary("DeleteBasket");

        }
    }
}
