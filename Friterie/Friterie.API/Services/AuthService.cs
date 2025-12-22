namespace Friterie.API.Services;

using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
    private readonly DataService _dataService;
    private readonly IConfiguration _configuration;
    private readonly IUserStore _userStore;

    public AuthService(IUserStore userStore, DataService dataService, IConfiguration configuration)
    {
        _dataService = dataService;
        _configuration = configuration;
        _userStore = userStore;
    }

    public async Task<User?> Register(string email, string password, string firstName, string lastName, string phoneNumber, string address)
    {
        var existingUser = await _userStore.GetUserByEmail(email);
        if (existingUser != null)
            return null;

        var user = new User
        {
            Email = email,
            Password = HashPassword(password),
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address
        };

        await _userStore.InsertUserAsync(user);

        return user;
    }

    public async Task<(User? user, string? token)> Login(string email, string password)
    {
        var user = await _userStore.GetUserByEmail(email);
        if (user == null || !VerifyPassword(password, user?.Password))
            return (null, null);

        var token = GenerateJwtToken(user);
        return (user, token);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:ExpiryInDays"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
