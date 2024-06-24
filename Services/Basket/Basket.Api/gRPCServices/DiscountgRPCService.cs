using Discount.gRPC.Protos;

namespace Basket.Api.gRPCServices
{
    public class DiscountgRPCService
    {
        #region Constructor

        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountgRPCService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        #endregion

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest
            {
                ProductName = productName
            };

            return await _discountProtoService
                .GetDiscountAsync(discountRequest);
        }
    }
}
