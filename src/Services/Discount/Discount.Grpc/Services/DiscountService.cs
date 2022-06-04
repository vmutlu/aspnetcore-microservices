using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Abstract;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<DiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName).ConfigureAwait(false);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var discountResponse = _mapper.Map<DiscountResponse>(coupon);
            return discountResponse;
        }

        public override async Task<DiscountResponse> AddDiscount(AddDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var addedCoupon = await _discountRepository.AddDiscount(coupon).ConfigureAwait(false);

            if (addedCoupon is false)
                throw new RpcException(new Status(StatusCode.NotFound, $"An unexpected error was encountered while adding Discount. ProductName={coupon.ProductName}"));

            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var discountResponse = _mapper.Map<DiscountResponse>(coupon);
            return discountResponse;
        }

        public override async Task<DiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var updatedCoupon = await _discountRepository.UpdateDiscount(coupon).ConfigureAwait(false);

            if (updatedCoupon is false)
                throw new RpcException(new Status(StatusCode.NotFound, $"An unexpected error was encountered while updating Discount. ProductName={coupon.ProductName}"));

            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var discountResponse = _mapper.Map<DiscountResponse>(coupon);
            return discountResponse;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.ProductName).ConfigureAwait(false);
            
            if (deleted is false)
                throw new RpcException(new Status(StatusCode.NotFound, $"An unexpected error was encountered while deleting Discount. ProductName={request.ProductName}"));
            
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
