using IdentityService.Entities;

namespace IdentityService.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}