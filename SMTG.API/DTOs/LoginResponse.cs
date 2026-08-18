namespace SMTG.API.DTOs;

public class LoginResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = "";

    public string? Token { get; set; }

    public string? UserId { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Role { get; set; }

    public List<string> Pages { get; set; } = new();

    public List<string> Actions { get; set; } = new();
}