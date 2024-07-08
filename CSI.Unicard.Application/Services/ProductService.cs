using AutoMapper;
using CSI.Unicard.Application.DTOs;
using CSI.Unicard.Application.Exceptions;
using CSI.Unicard.Application.FluentValidates;
using CSI.Unicard.Application.Interfaces;
using CSI.Unicard.Application.StaticFiles;
using CSI.Unicard.Domain.Interfaces;
using CSI.Unicard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CSI.Unicard.Application.Services
{
    public class ProductService : AbstractService,IProductService
    {
        private readonly ValidateProduct validationRules;
        private readonly IMapper mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this.validationRules = new ValidateProduct();
            this.mapper=mapper;
        }

        #region Add
        public async Task<bool> Add(ProductDTO entity)
        {
            var validator=validationRules.Validate(entity);
            if (!validator.IsValid) throw new ValidationException(ErrorKeys.BadRequest);
            var mapped = mapper.Map<Products>(entity)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            var res=await unitOfWork.products.Add(mapped);
            return res;
        }
        #endregion

        #region DeleteById
        public async Task DeleteById(int id)
        {
            if (id < 0) throw new ArgumentException("Invalid argument");
            await unitOfWork.products.DeleteById(id);
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var res=await unitOfWork.products.GetAll();
            if (!res.Any()) throw new NotFoundException(ErrorKeys.NotFound);
            var mapped=mapper.Map<IEnumerable<ProductDTO>>(res)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region GetById
        public async Task<ProductDTO> GetById(int id)
        {
            if (id < 0) throw new ArgumentException("Invalid input parameter");
            var res=await unitOfWork.products.GetById(id);
            var mapped = mapper.Map<ProductDTO>(res)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion
    }
}
