using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CSI.Unicard.Infrastructure.Repositories
{
    public class UserRepository : AbstractRepository, IUser
    {
        public UserRepository(IConfiguration _configuration) : base(_configuration)
        {
        }

        #region Add
        public async Task<bool> Add(Users entity)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using(var connection=new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO [dbo].[Users]
                          ([UserName]
                         ,[Password]
                         ,[Email])
                          VALUES
                          (@UserName,@Email,@Password)";
                var user = new
                {
                    entity.UserName,
                    entity.Email,
                    entity.Password
                };
                await connection.ExecuteAsync(sql, user);
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
                var sql = @"delete from Users
                            where Users.UserId=@id";
                await connection.ExecuteAsync(sql, new { id });
            }
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<Users>> GetAll()
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using(var connetion=new SqlConnection(connectionString))
            {
                await connetion.OpenAsync();
                var sql = "select * from Users";
                var res=await connetion.QueryAsync<Users>(sql);
                return res is null ? throw new InvalidOperationException("No user found in database")
                    : res;
            }
        }
        #endregion

        #region GetById
        public async Task<Users> GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("UnicardDb");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string sql = @"
                           select * from Users
                           where Users.UserId== @id";

                var user = await connection.QueryFirstOrDefaultAsync<Users>(sql, new { id });

                if (user == null)
                {
                    Console.WriteLine($"No user found with user: {id}");
                }

                return user ?? throw new InvalidOperationException($"No user found by id: {id}");
            }
        }
        #endregion
    }
}
