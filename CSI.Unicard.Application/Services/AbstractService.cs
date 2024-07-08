using CSI.Unicard.Domain.Interfaces;

namespace CSI.Unicard.Application.Services
{
    public class AbstractService
    {
        protected IUnitOfWork unitOfWork { get; set; }

        public AbstractService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
