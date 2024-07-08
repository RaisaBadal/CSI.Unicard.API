using CSI.Unicard.Application.Exceptions;
using CSI.Unicard.Application.FluentValidates;
using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;

namespace CSI.Unicard.Application.Services
{
    public class UserService : AbstractService, IUserService
    {
        private readonly ValidateUser validationRules;
        public UserService(Domain.Interfaces.IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.validationRules = new ValidateUser();
        }

        #region Add
        public async Task<bool> Add(Users entity)
        {
            var validator = validationRules.Validate(entity);
            if (!validator.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await unitOfWork.user.Add(entity);
            return res;
        }
        #endregion

        #region DeleteById
        public async Task DeleteById(int id)
        {
            if (id < 0) throw new ArgumentException("Invalid argument");
            await unitOfWork.user.DeleteById(id);
        }
        #endregion

        #region GetAll

        public async Task<IEnumerable<Users>> GetAll()
        {
            var res = await unitOfWork.user.GetAll();
            if (!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
            return res;
        }
        #endregion

        #region GetById
        public async Task<Users> GetById(int id)
        {
            if (id < 0) throw new ArgumentException("Invalid input parameter");
            var res = await unitOfWork.user.GetById(id);
            return res is null ? throw new NotFoundException() : res;
        }
        #endregion
    }
}
