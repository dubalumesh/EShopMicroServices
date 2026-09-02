
using Catlog.API.Models;

namespace Catlog.API.Products.GetProducts;

public record GetProductsQuery() :
      IQuery<GetProductsQueryResult>;


public record GetProductsQueryResult(IEnumerable<Product> products);

public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Calling GetProductsQueryHandler.Handle ");
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsQueryResult(products);
    }
}

