using Discount.API.Entities;
using System.Threading.Tasks;

namespace Discount.API.Repositories.Abstract
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> AddDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
