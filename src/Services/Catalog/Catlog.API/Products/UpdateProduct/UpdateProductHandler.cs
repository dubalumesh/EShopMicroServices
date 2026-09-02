
using Catlog.API.Models;
using Catlog.API.Products.GetProducts;

namespace Catlog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, List<string> Category, string ImageFile, Decimal Price)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSucess);

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Executing UpdateProductCommandHandler.Handle with parameter {@command}", command);
            var product = await session.Query<Product>().Where(x => x.Id == command.Id).FirstOrDefaultAsync();
            if (product is null)
                throw new ProductNotFoundException($"Product with Id '{command.Id}' was not found.");
            product.Name = command.Name;
            product.Description = command.Description;
            product.Category = command.Category;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);

        }
    }
}
