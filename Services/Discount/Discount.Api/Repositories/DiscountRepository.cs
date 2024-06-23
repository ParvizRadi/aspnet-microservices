﻿using Discount.Api.Entities;
using Npgsql;
using Dapper;


namespace Discount.Api.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        #region Constructor

        private readonly IConfiguration _configuration;

        NpgsqlConnection _connection = new NpgsqlConnection();

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection
                (
                    _configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                );
        }

        #endregion

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await _connection.QueryFirstOrDefaultAsync
                (
                    "SELECT * FROM Coupon " +
                    "WHERE ProductName = @ProductName",
                    new { ProductName = productName }
                );

            if (coupon == null)
            {
                return new Coupon
                {
                    ProductName = "No Discount",
                    Description = "No Description",
                    Amount = 0,
                };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync
                (
                    "INSERT INTO Coupon " +
                    "(ProductName, Description, Amount) " +
                    "VALUES" +
                    "(@ProductName,@Description,@Amount)",
                    new
                    {
                        ProductName = coupon.ProductName,
                        Description = coupon.Description,
                        Amount = coupon.Amount,
                    }
                );

            return affected > 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync
                (
                    "UPDATE coupon SET Productname=@Productname ,Description =@Description,Amount = @Amount WHERE ID= @ID",
                    new
                    {
                        ProductName = coupon.ProductName,
                        Description = coupon.Description,
                        Amount = coupon.Amount,
                        ID = coupon.Id
                    }
                );

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var affected = await _connection.ExecuteAsync
                (
                    "DELETE FROM coupon WHERE ProductName= @ProductName",
                    new
                    {
                        ProductName = productName,
                    }
                );

            return affected > 0;
        }
    }
}
