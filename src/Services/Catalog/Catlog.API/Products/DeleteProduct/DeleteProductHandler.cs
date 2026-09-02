
using Catlog.API.Models;
using Catlog.API.Products.UpdateProduct;

namespace Catlog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductCommandResult>;

    public record DeleteProductCommandResult(bool IsScuccess);

    internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
    {
        public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Executing DeleteProductCommandHandler.Handle with parameter {@command}", command);
            var product = await session.Query<Product>().Where(p => p.Id == command.Id).FirstOrDefaultAsync();
            if (product is null)
                throw new ProductNotFoundException($"Product with Id '{command.Id}' was not found.");
            session.Delete(product);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductCommandResult(true);
        }
    }
}
