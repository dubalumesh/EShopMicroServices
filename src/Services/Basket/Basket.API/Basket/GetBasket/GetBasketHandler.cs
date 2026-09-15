
namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketQueryResult>;

    public record GetBasketQueryResult(ShoppingCart ShoppingCart);

    public class GetBasketQueryHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketQueryResult>
    {
        public async Task<GetBasketQueryResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {

            var basket = await basketRepository.GetBasket(request.UserName, cancellationToken);
            return new GetBasketQueryResult(basket);
        }
    }
}
