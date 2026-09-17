
using Discount.Grpc;

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



    internal class StoreBasketCommandHandler(IBasketRepository basketRepository,
        DiscountProtoService.DiscountProtoServiceClient discountServiceClient
        ) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await ApplyDiscount(command, cancellationToken);
            var createdCart = await basketRepository.StoreBasket(command.cart, cancellationToken); // Store the shopping cart in the repository

            var response = new StoreBasketResult(createdCart.UserName); // This needs to update to use the actual user name from the cart, e.g., cart.UserName
            return response;
        }

        private async Task ApplyDiscount(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            //todo: To communicate with Discount gRPC service to calculate the total price of the shopping cart, you can use a gRPC client to call the appropriate method on the Discount service. This will allow you to retrieve any applicable discounts and update the total price of the shopping cart accordingly.
            foreach (var item in command.cart.Items)
            {
                var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };
                var discountResponse = await discountServiceClient.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);
                item.Price -= Convert.ToDecimal(discountResponse.Amount); // Apply the discount to the item's price
            }
        }
    }
}
