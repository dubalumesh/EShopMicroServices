
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart cart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.cart).NotNull().WithMessage("Shopping cart cannot be null.");
            RuleFor(x => x.cart.UserName).NotEmpty().WithMessage("User name cannot be empty.");
        }
    }



    internal class StoreBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.cart;
            var createdCart = await basketRepository.StoreBasket(cart, cancellationToken); // Store the shopping cart in the repository

            var response = new StoreBasketResult(cart.UserName); // This needs to update to use the actual user name from the cart, e.g., cart.UserName
            return response;
        }
    }
}
