


using Basket.API.Exceptions;

namespace Basket.API.Data
{
    internal class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            var cart = session.Query<ShoppingCart>().FirstOrDefault(x => x.UserName == userName);
            if (cart != null)
            {
                session.Delete(cart);
                await session.SaveChangesAsync(cancellationToken);
                return true;

            }
            return false;

        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var cart = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
            if (cart is null)
                throw new BasketNotFoundException();
            return cart;
        }

        public Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
        {
            session.Store(basket);
            return session.SaveChangesAsync(cancellationToken)
                .ContinueWith(_ => basket, cancellationToken);

        }
    }
}
