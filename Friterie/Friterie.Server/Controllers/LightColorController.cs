using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]")]
public class LightColorController : ControllerBase
{
    private static readonly string[] Colors = new[]
    {
        "#FFCCCC", "#CCFFCC", "#CCCCFF", "#FFFFCC", "#CCFFFF"
    };

    [HttpGet]
    public string Get()
    {
        var rnd = new Random();
        return Colors[rnd.Next(Colors.Length)];
    }
}