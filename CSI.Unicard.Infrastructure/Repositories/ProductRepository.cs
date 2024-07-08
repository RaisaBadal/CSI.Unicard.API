using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CSI.Unicard.Infrastructure.Repositories
{
    public class ProductRepository : AbstractRepository, IProducts
    {
        public ProductRepository(IConfiguration _configuration) : base(_configuration)
        {
        }

        #region Add
        public async Task<bool> Add(Products entity)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO [dbo].[Products]
                    ([ProductName]
                    ,[Description]
                    ,[Price])
                     VALUES
                     ()";
                var product = new
                {
                    entity.ProductName,
                    entity.Description,
                    entity.Price
                };
                await connection.ExecuteAsync(sql, product);
                return true;
            }
        }
        #endregion

        #region DeleteById
        public async Task DeleteById(int id)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"delete from Products
                            where Products.ProductId=@id";
                await connection.ExecuteAsync(sql, new { id });
            }
        }

        #endregion

        #region GetAll
        public async Task<IEnumerable<Products>> GetAll()
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection=new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var products = await connection.QueryAsync<Products>("select * from Products");
                if (products != null) return products;
                else throw new InvalidOperationException("No product found in database");
            }
        }
        #endregion

        #region GetById
        public async Task<Products> GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"select * from Products where ProductId=@id";
                var product = await connection.QueryFirstOrDefaultAsync<Products>(sql, new { id });
                if (product != null) return product;
                else throw new InvalidOperationException($"No product found in database by id: {id}");
            }
        }
        #endregion
    }
}
