using BuildingBlocks.CQRS;
using Catlog.API.Models;


namespace Catlog.API.Products.CreateProduct
{

    public record CreateProductCommand(string Name, string Description, List<string> Category, string ImageFile, Decimal Price) :
        ICommand<CreateProductCommandResult>;
    public record CreateProductCommandResult(Guid Id);

    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Product category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Product image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
    {
        public async Task<CreateProductCommandResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {


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
