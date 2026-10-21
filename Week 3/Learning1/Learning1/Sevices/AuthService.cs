using StudentApi.Models;

namespace StudentApi.Services;

public class AuthService(JwtService jwtService)
{
    public LoginResponse? Login(LoginRequest request)
    {
        string role;

        if (request.Username == "admin" &&
            request.Password == "Admin@123")
        {
            role = "Admin";
        }
        else if (request.Username == "student" &&
                 request.Password == "Student@123")
        {
            role = "Student";
        }
        else
        {
            return null;
        }

        var result = jwtService.GenerateToken(
            request.Username, role);

        return new LoginResponse
        {
            Token = result.Token,
            Username = request.Username,
            Role = role,
            ExpiresAt = result.ExpiresAt
        };
    }
}