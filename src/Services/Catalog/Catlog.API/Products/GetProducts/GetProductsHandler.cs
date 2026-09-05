
using Catlog.API.Models;
using Marten.Pagination;

namespace Catlog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber, int? PageSize = 10) :
      IQuery<GetProductsQueryResult>;


public record GetProductsQueryResult(IEnumerable<Product> products);

public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
        return new GetProductsQueryResult(products);
    }
}

