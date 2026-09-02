namespace Catlog.API.Exceptions
{
    public class ProductNotFoundException : Exception
    {

        public ProductNotFoundException() : base("product not found.") { }
        public ProductNotFoundException(string message) : base(message) { }
    }
}
