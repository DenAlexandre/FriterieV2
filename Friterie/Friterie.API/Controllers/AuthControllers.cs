using Friterie.API.DTOs;
using Friterie.API.Services;
using Friterie.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly AuthStateService _authState;

    private const string GET_LOGIN = "FriterieAPI/api/auth/login";
    private const string GET_LOGOUT = "FriterieAPI/api/auth/logout";
    private const string REGISTER_BDD = "FriterieAPI/api/auth/register";

    public AuthController(AuthService authService, AuthStateService authState)
    {
        _authService = authService;
        _authState = authState;
    }


    [HttpPost(REGISTER_BDD)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _authService.Register(dto.Email, dto.Password, dto.FirstName, dto.LastName, dto.PhoneNumber, dto.Address);

        if (user == null)
            return BadRequest(new { message = "Un utilisateur avec cet email existe déjà" });

        return Ok(new { message = "Inscription réussie", userId = user.UserId });
    }



    [HttpPost(GET_LOGIN)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (user, token) = await _authService.Login(dto.Email, dto.Password);

        if (user == null || token == null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });

        // Claims pour le cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Création du cookie côté serveur
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        // Stockage du token côté serveur (Scoped service)
        _authState.SetUser(principal, token);


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
        });






        return Ok(new { token, user });
    }

    [HttpPost(GET_LOGOUT)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _authState.Clear();
        return Ok();
    }
}
