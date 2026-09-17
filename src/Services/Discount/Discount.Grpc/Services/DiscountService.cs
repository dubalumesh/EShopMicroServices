
namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext discountContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CuponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
                coupon = new Coupon() { ProductName = "No Discount", Amount = 0, Description = "Discount is not applicable" };

            logger.LogInformation("Discount is retrived for ProductName: {ProductName} of Amount:{Amount} ", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CuponModel>();

        }

        public override async Task<CuponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var newCoupon = request.Cupon.Adapt<Coupon>();

            if (newCoupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));

            discountContext.Coupons.Add(newCoupon);
            await discountContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", newCoupon.ProductName);

            return newCoupon.Adapt<CuponModel>();
        }

        public override async Task<CuponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var requestCoupon = request.Cupon.Adapt<Coupon>();

            if (requestCoupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));

            var coupon = await discountContext.Coupons.FirstOrDefaultAsync(c => c.Id == requestCoupon.Id);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Id={requestCoupon.Id} is not found."));

            coupon.ProductName = requestCoupon.ProductName;
            coupon.Description = requestCoupon.Description;
            coupon.Amount = requestCoupon.Amount;

            discountContext.Coupons.Update(coupon);
            await discountContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}", coupon.ProductName);

            return coupon.Adapt<CuponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with ProductName={request.ProductName} is not found."));

            discountContext.Coupons.Remove(coupon);
            await discountContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully deleted. ProductName: {ProductName}", coupon.ProductName);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}