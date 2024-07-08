namespace CSI.Unicard.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IOrders orders { get; }

        public IProducts products { get; }

        public IUser user { get; }

        public IOrderItem item { get; }
    }
}
