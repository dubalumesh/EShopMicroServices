
using Catlog.API.Models;
namespace Catlog.API.Products.GetProductById;

//public record GetProductRequest(Guid Id);
public record GetProductResponse(Product product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/product/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            var response = result.Adapt<GetProductResponse>();
            return Results.Ok(response);

        }).WithName("GetProductById")
                .Produces<GetProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("GetProductById")
                .WithSummary("GetProductById");
    }
}

