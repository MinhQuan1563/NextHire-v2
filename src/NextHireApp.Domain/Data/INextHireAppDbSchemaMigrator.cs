using System.Threading.Tasks;

namespace NextHireApp.Data;

public interface INextHireAppDbSchemaMigrator
{
    Task MigrateAsync();
}
