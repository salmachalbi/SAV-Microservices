using AuthService.Data;
using AuthService.DTOs;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthDbContext _ctx;
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;

    public AuthController(AuthDbContext ctx,
                          PasswordService passwordService,
                          TokenService tokenService,
                          IHttpClientFactory httpClientFactory)
    {
        _ctx = ctx;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (_ctx.Users.Any(u => u.Username == dto.Username))
            return BadRequest("Username already exists");

        _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = "Client"
        };

        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();

        // 🔥 Création Client dans ClientService
        var http = _httpClientFactory.CreateClient();

        var clientDto = new
        {
            UserId = user.Id,
            Nom = dto.Username,
            Prenom = "Client",
            Email = dto.Email
        };

        var response = await http.PostAsJsonAsync(
            "https://localhost:7266/api/Clients",
            clientDto
        );

        if (!response.IsSuccessStatusCode)
            return StatusCode(500, "Erreur lors de la création du client dans ClientService");

        return Ok("Client registered successfully");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _ctx.Users.SingleOrDefault(u => u.Username == dto.Username);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!_passwordService.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized("Invalid credentials");

        var token = _tokenService.GenerateToken(user);

        return Ok(new
        {
            token,
            role = user.Role,
            username = user.Username
        });
    }
}
