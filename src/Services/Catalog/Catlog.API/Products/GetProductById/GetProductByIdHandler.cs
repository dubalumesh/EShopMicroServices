using Catlog.API.Models;
namespace Catlog.API.Products.GetProductById;
public record GetProductByIdQuery(Guid Id) :
      IQuery<GetProductByIdQueryResult>;

public record GetProductByIdQueryResult(Product product);

internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) :
    IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult>
{
    public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Calling GetProductByIdQueryHandler.Handle for Id: {Id}", query.Id);
        var product = await session.Query<Product>().Where(x => x.Id == query.Id).FirstOrDefaultAsync();
        if (product == null)
            throw new ProductNotFoundException($"Product with Id '{query.Id}' was not found.");
        return new GetProductByIdQueryResult(product);
    }
}

