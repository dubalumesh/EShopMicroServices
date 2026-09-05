
using Catlog.API.Models;
using Catlog.API.Products.CreateProduct;

namespace Catlog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber, int? PageSize = 10);
public record GetProductsResponse(IEnumerable<Product> products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {

            var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(query);

            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);


        }).WithName("GetProducts")
                .Produces<GetProductsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("GetProducts")
                .WithSummary("GetProducts");
    }
}

