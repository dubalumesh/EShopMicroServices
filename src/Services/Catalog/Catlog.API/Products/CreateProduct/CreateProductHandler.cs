using BuildingBlocks.CQRS;
using Catlog.API.Models;


namespace Catlog.API.Products.CreateProduct
{

    public record CreateProductCommand(string Name, string Description, List<string> Category, string ImageFile, Decimal Price) :
        ICommand<CreateProductCommandResult>;
    public record CreateProductCommandResult(Guid Id);
    internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
    {
        public async Task<CreateProductCommandResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Executing CreateProductCommandHandler.Handle with parameter {@command}", command);
            //create product entity
            var product = new Product()
            {
                Name = command.Name,
                Description = command.Description,
                Category = command.Category,
                ImageFile = command.ImageFile,
                Price = command.Price

            };

            // save product entity to DB
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductCoammndResult
            return new CreateProductCommandResult(product.Id);



        }
    }
}
