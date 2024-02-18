namespace AuthMicro.Identity;

public interface IAuthentication
{
    Task<string> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task RefreshTokenAsync(string ExpireToken);
}


public record LoginRequest(string Email, string Password, bool? RememberMe, bool? ForgetPassword);
public record RegisterRequest(string Email, string Password);