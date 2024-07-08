using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CSI.Unicard.Infrastructure.Repositories
{
    public class OrderItemRepository : AbstractRepository, IOrderItem
    {
        public OrderItemRepository(IConfiguration _configuration) : base(_configuration)
        {
        }

        #region Add
        public async Task<bool> Add(OrderItems entity)
        {
          
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO [dbo].[OrderItems]
                     ([OrderId]
                     ,[ProductId]
                     ,[Quantity]
                     ,[Price])
                      VALUES
                      (@OrderId,@ProductId,@Quantity,@price)";
                var parameters = new
                {
                    entity.OrderId,
                    entity.ProductId,
                    entity.Quantity,
                    entity.price
                };
                await connection.ExecuteAsync(sql, parameters);
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
                var sql = @"delete from OrderItems
                            where OrderItems.OrderItemId=@id";
                await connection.ExecuteAsync(sql, new { id });
            }
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<OrderItems>> GetAll()
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"select * from OrderItems orit 
                           join Orders ord on orit.OrderId=ord.OrderId
                           join Products pr on orit.ProductId=pr.ProductId";
                var res = await connection.QueryAsync<OrderItems>(sql);
                return res is null ? throw new InvalidOperationException("No OrderItems found in database")
                    : res;
            }
        }
        #endregion

        #region GetById
        public async Task<OrderItems> GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string sql = @"
                          select * from OrderItems
                          where OrderItems.OrderItemId=@id";

                var user = await connection.QueryFirstOrDefaultAsync<OrderItems>(sql, new { id });

                return user ?? throw new InvalidOperationException($"No OrderItems found by id: {id}");
            }
        }
        #endregion
    }
}
