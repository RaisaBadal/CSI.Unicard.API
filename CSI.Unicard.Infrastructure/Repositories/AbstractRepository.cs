using Microsoft.Extensions.Configuration;

namespace CSI.Unicard.Infrastructure.Repositories
{
    public abstract class AbstractRepository
    {
        public readonly IConfiguration _configuration;

        protected AbstractRepository(IConfiguration _configuration)
        {
              this._configuration = _configuration;
        }

    }
}
