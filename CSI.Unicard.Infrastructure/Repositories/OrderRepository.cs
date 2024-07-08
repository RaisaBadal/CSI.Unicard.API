using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CSI.Unicard.Infrastructure.Repositories
{
    public class OrderRepository : AbstractRepository, IOrders
    {
        public OrderRepository(IConfiguration _configuration) : base(_configuration)
        {
        }

        #region Add
        public async Task<bool> Add(Orders entity)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO [dbo].[Orders] (UserId, OrderDate, TotalAmount) 
                    VALUES (@UserId, @OrderDate, @TotalAmount)";
                var parameters = new
                {
                    entity.UserId,
                    entity.OrderDate,
                    entity.TotalAmount
                };
                await connection.ExecuteAsync(sql, parameters);
                return true;

            }
        }
        #endregion

        #region GetById
        public async Task<Orders> GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string sql = @"
                             select * from Orders
                             join Users on Orders.UserId=Users.UserId
                             where Orders.OrderId= @id";

          

                Console.WriteLine($"Executing query: {sql} with parameter id: {id}");

                var order = await connection.QueryFirstOrDefaultAsync<Orders>(sql, new { id });

                if (order == null)
                {
                    Console.WriteLine($"No order found with OrderItemId: {id}");
                }

                return order ?? throw new InvalidOperationException($"No order found by id: {id}");
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
                var sql = @"delete from Orders
                            where Orders.OrderId=@id";
                await connection.ExecuteAsync(sql, new { id });
            }
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<Orders>> GetAll()
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using(var connection= new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = "select * from Orders";
                var res = await connection.QueryAsync<Orders>(sql);
                return res is null ? throw new InvalidOperationException("No order found in database")
                    : res;
            }
        }
        #endregion
    }
}
