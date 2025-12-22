namespace Friterie.API.Controllers;

using Friterie.API.DTOs;
using Friterie.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("FriterieAPI/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _authService.Register(dto.Email, dto.Password, dto.FirstName, dto.LastName, dto.PhoneNumber, dto.Address);

        if (user == null)
            return BadRequest(new { message = "Un utilisateur avec cet email existe déjà" });

        return Ok(new { message = "Inscription réussie", userId = user.UserId});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (user, token) = await _authService.Login(dto.Email, dto.Password);

        if (user == null || token == null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });

        return Ok(new
        {
            token,
            user = new
            {
                user.UserId,
                user.Email,
                user.FirstName,
                user.LastName
            }
        });
    }
}
