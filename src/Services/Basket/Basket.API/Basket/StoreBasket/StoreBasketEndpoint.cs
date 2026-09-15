
using static Basket.API.Basket.GetBasket.GetBasketEndpoint;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart cart);

    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
             {
                 var command = new StoreBasketCommand(request.cart);
                 var result = await sender.Send(command, CancellationToken.None);

                 StoreBasketResponse response = result.Adapt<StoreBasketResponse>();

                 return Results.Created($"/basket/{response.UserName}", response);
             })
             .WithName("StoreBasket")
              .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("StoreBasket")
                .WithSummary("StoreBasket");


        }
    }
}
