using CSI.Unicard.Application.Exceptions;
using CSI.Unicard.Application.FluentValidates;
using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;

namespace CSI.Unicard.Application.Services
{
    public class OrderItemService : AbstractService, IOrderItemService
    {
        private readonly ValidateOrderItem validationRules;
        public OrderItemService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            validationRules=new ValidateOrderItem();
        }

        #region Add
        public async Task<bool> Add(OrderItems entity)
        {
            var validate = validationRules.Validate(entity);
            if (!validate.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);

            var order = await unitOfWork.orders.GetById(entity.OrderId)
                ?? throw new InvalidOperationException($"No order found by id: {entity.OrderId}");

            var product=await unitOfWork.products.GetById(entity.ProductId)
                ?? throw new InvalidOperationException($"No product found by id: {entity.ProductId}");

            var res=await unitOfWork.item.Add(entity);
            return res;
        }
        #endregion

        #region DeleteById
        public async Task DeleteById(int id)
        {
            if (id < 0) throw new ArgumentException("Invalid argument");
            await unitOfWork.item.DeleteById(id);
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<OrderItems>> GetAll()
        {
            var res = await unitOfWork.item.GetAll();
            if (!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
            return res;
        }
        #endregion

        #region GetById

        public async Task<OrderItems> GetById(int id)
        {
            if (id < 0) throw new ArgumentException("Invalid input parameter");
            var res = await unitOfWork.item.GetById(id);
            return res is null ? throw new NotFoundException() : res;
        }
        #endregion
    }
}
