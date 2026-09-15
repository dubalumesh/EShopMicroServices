namespace Basket.API.Basket.GetBasket
{
    public class GetBasketEndpoint : ICarterModule
    {

        //public record GetBasketRequest(string UserName);

        public record GetBasketResponse(ShoppingCart ShoppingCart);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
                {
                    var result = await sender.Send(new GetBasketQuery(userName));
                    return Results.Ok(result);
                }).WithName("GetBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("GetBasket")
                .WithSummary("GetBasket");
        }
    }
}
