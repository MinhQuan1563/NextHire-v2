using NextHireApp.Auth.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace NextHireApp.Auth.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> IssueAsync(IdentityUser user, string? oldRefreshToken = null);
    }
}
