namespace Friterie.API.Controllers;

using Friterie.API.DTOs;
using Friterie.API.Services;
using Friterie.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
public class AuthController : ControllerBase
{

    private const string GET_LOGIN_BDD = "FriterieAPI/api/auth/login";
    private const string REGISTER_BDD = "FriterieAPI/api/auth/register";


    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost(REGISTER_BDD)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _authService.Register(dto.Email, dto.Password, dto.FirstName, dto.LastName, dto.PhoneNumber, dto.Address);

        if (user == null)
            return BadRequest(new { message = "Un utilisateur avec cet email existe déjà" });

        return Ok(new { message = "Inscription réussie", userId = user.UserId});
    }

    [HttpPost(GET_LOGIN_BDD)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (user, token) = await _authService.Login(dto.Email, dto.Password);

        if (user == null || token == null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });

        return Ok(new
        {
            token,
            user = new User
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                RoleId = user.RoleId,
                RoleName = user.RoleName
            }

            //user = new
            //{
            //    user.UserId,
            //    user.Email,
            //    user.FirstName,
            //    user.LastName
            //}
        });
    }
}
