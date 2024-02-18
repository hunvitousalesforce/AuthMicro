
using System.Runtime.Serialization;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AuthMicro.Identity;

public class AuthenticationService : IAuthentication
{
    private readonly UserManager<ApplicationUser> _UserManager;
    private readonly SignInManager<ApplicationUser> _SignInManager;
    private readonly ILogger<ApplicationUser> _Logger;
    private readonly IPasswordHasher<ApplicationUser> _PasswordHasher;
    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<ApplicationUser> logger,
        IPasswordHasher<ApplicationUser> passwordHasher
    )
    {
        _UserManager = userManager;
        _SignInManager = signInManager;
        _Logger = logger;
        _PasswordHasher = passwordHasher;
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await _UserManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new LoginFailedException("login failed");

        var result = await _SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (result.Succeeded)
        {
            var token = JwtServices.GenerateToken(user);
            return token;
        }
        throw new LoginFailedException("Invalid login attempt.");
    }

    public Task RefreshTokenAsync(string ExpireToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // check if user already exist
        var userFromDb = await _UserManager.FindByEmailAsync(request.Email);
        if (userFromDb != null)
            throw new UserAlreadyExistException("this user already exists");
        try
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
            };
            user.PasswordHash = _PasswordHasher.HashPassword(user, request.Password);
            var result = await _UserManager.CreateAsync(user);
            if (result.Succeeded)
                return true;
        }
        catch (Exception e)
        {
            _Logger.LogError(e.Message);
        }
        return false;
    }
}