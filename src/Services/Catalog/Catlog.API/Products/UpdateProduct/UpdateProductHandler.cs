
using Catlog.API.Models;
using Catlog.API.Products.GetProducts;

namespace Catlog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, List<string> Category, string ImageFile, Decimal Price)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSucess);


    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Product category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Product image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
        }
    }
    internal class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
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
