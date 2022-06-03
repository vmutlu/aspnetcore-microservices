using Dapper;
using Discount.API.Entities;
using Discount.API.Repositories.Abstract;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories.Concrete
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> AddDiscount(Coupon coupon)
        {
            using var connection = OpenConnection();

            var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description,Amount) VALUES (@ProductName,@Description,@Amount)", new { ProductName = coupon.ProductName,Description = coupon.Description,Amount = coupon.Amount }).ConfigureAwait(false);

            if (affected is 0) return false;

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = OpenConnection();

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName}).ConfigureAwait(false);

            if (affected is 0) return false;

            return affected > 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = OpenConnection();

            var coupen = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE productName = @productName", new { productName }).ConfigureAwait(false);

            if (coupen is null) return new Coupon() { ProductName = productName, Amount = 0, Description = "No discount" };

            return coupen;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = OpenConnection();

            var affected = await connection.ExecuteAsync("UPDATE COUPON SET ProductName = @ProductName, Description = @Description,Amount=@Amount WHERE Id = @Id", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id }).ConfigureAwait(false);

            if (affected is 0) return false;

            return affected > 0;
        }

        private NpgsqlConnection OpenConnection() => new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    }
}
