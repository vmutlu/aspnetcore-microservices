using Discount.API.Entities;
using Discount.API.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountsController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var coupon = await _discountRepository.GetDiscount(productName).ConfigureAwait(false);
            
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> AddDiscount([FromBody] Coupon coupon)
        {
            await _discountRepository.AddDiscount(coupon).ConfigureAwait(false);

            return CreatedAtRoute(nameof(GetDiscount), new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(coupon).ConfigureAwait(false));
        }

        [HttpDelete("{productName}")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string productName)
        {
            return Ok(await _discountRepository.DeleteDiscount(productName).ConfigureAwait(false));
        }
    }
}
