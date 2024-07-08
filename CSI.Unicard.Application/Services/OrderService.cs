using CSI.Unicard.Application.Exceptions;
using CSI.Unicard.Application.FluentValidates;
using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CSI.Unicard.Application.Services
{
    public class OrderService : AbstractService, IOrderService
    {
        public ValidateOrder validationRules {  get; set; }
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.validationRules = new ValidateOrder();
        }

        #region Add

        public async Task<bool> Add(Orders orders)
        {
            var validator=validationRules.Validate(orders);
            if (!validator.IsValid) throw new ValidationException(ErrorKeys.BadRequest);
            var res=await unitOfWork.orders.Add(orders);
            return res;
        }
        #endregion

        #region GetById
        public async Task<Orders> GetById(int id)
        {
            var res=await unitOfWork.orders.GetById(id);
            return res is null ? throw new NotFoundException(ErrorKeys.BadRequest)
                : res;
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<Orders>> GetAll()
        {
            var res = await unitOfWork.orders.GetAll();
            if (!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
            return res;
        }
        #endregion

        #region DeleteById
        public async Task DeleteById(int id)
        {
            await unitOfWork.orders.DeleteById(id);
        }
        #endregion
    }
}
