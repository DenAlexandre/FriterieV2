namespace Friterie.API.Services;

using Friterie.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService
{
    private readonly DataService _dataService;
    private readonly IConfiguration _configuration;

    public AuthService(DataService dataService, IConfiguration configuration)
    {
        _dataService = dataService;
        _configuration = configuration;
    }

    public User? Register(string email, string password, string firstName, string lastName, string phoneNumber, string address)
    {
        var existingUser = _dataService.GetUserByEmail(email);
        if (existingUser != null)
            return null;

        var user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password),
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address
        };

        return _dataService.AddUser(user);
    }

    public (User? user, string? token) Login(string email, string password)
    {
        var user = _dataService.GetUserByEmail(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return (null, null);

        var token = GenerateJwtToken(user);
        return (user, token);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
