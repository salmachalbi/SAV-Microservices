using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


// 🔹 Charger Ocelot.json
builder.Configuration.AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true);

// 🔹 Services standards
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔐 JWT Authentication (obligatoire pour microservices sécurisés)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();

// 🔹 Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

// 🔹 Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// 🔐 Ordre important : Authentification puis Autorisation
app.UseAuthentication();
app.UseAuthorization();

// 🔹 Ocelot doit être avant MapControllers pour que le routage fonctionne
await app.UseOcelot();

// 🔹 MapControllers pour les endpoints propres à la Gateway (optionnel)
app.MapControllers();

app.Run();
