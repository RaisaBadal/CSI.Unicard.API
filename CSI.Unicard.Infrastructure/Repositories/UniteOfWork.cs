using CSI.Unicard.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CSI.Unicard.Infrastructure.Repositories
{
    public class UniteOfWork : IUnitOfWork
    {
        public readonly IConfiguration configuration;

        public UniteOfWork(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IOrders orders => new OrderRepository(configuration);

        public IProducts products => new ProductRepository(configuration);

        public IUser user => new UserRepository(configuration);

        public IOrderItem item => new OrderItemRepository(configuration);
    }
}
