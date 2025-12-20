using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]")]
public class DarkColorController : ControllerBase
{
    [HttpGet("{level}")]
    public string Get(int level)
    {
        level = Math.Clamp(level, 1, 10);
        int shade = 255 - (level * 20);
        return $"#{shade:X2}{shade:X2}{shade:X2}";
    }
}