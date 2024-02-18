namespace Domain;

public class LoginFailedException : Exception
{
    public LoginFailedException(string? message) : base(message) { }
}
