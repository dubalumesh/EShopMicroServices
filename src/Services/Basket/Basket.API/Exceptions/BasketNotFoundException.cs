using System;

namespace Basket.API.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException() : base("basket not found.") { }
        public BasketNotFoundException(string message) : base(message) { }
    }
}
