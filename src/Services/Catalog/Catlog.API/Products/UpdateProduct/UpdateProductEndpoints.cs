
using Catlog.API.Products.CreateProduct;
using Catlog.API.Products.GetProductByCategory;

namespace Catlog.API.Products.UpdateProduct
{

    public record UpdateProductRequest(Guid Id, string Name, string Description, List<string> Category, string ImageFile, Decimal Price);

    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("Products/", async (UpdateProductRequest request, ISender sender) =>
            {

                var result = await sender.Send(request.Adapt<UpdateProductCommand>());
                var response = new UpdateProductResponse(result.IsSucess);
                return Results.Ok(response);

            }).WithName("UpdateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("UpdateProduct")
                .WithSummary("UpdateProduct");
        }
    }
}
