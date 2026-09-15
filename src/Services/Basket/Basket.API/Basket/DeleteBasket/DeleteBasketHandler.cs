
using MediatR;

namespace Basket.API.Basket.DeleteBasket
{

    public record DeleteBasketCommand(string userName) : ICommand<DeleteBasketCommandResult>;

    public record DeleteBasketCommandResult(bool IsSucess);

    public class DeleteBasketValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketValidator()
        {
            RuleFor(x => x.userName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class DeleteBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketCommandResult>
    {
        public async Task<DeleteBasketCommandResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.DeleteBasket(command.userName, cancellationToken);
            return new DeleteBasketCommandResult(basket);
        }
    }
}
