using System.Threading.Tasks;

namespace ErpShowroom.Application.sys.Interfaces;

public record AuthResult(string AccessToken, string RefreshToken);
public record LoginRequest(string Username, string Password);
public record RefreshRequest(string AccessToken, string RefreshToken);

public interface IAuthService
{
    Task<AuthResult?> LoginAsync(LoginRequest request);
    Task<AuthResult?> RefreshAsync(RefreshRequest request);
    Task<bool> LogoutAsync(string username);
}
