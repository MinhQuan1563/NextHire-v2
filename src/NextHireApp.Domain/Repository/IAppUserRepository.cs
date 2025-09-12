using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.Repository
{
    public interface IAppUserRepository : ITransientDependency
    {
        Task<bool> IsExists(string userCode);
    }
}
