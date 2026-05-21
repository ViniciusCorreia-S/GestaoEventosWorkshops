using System.Text;
using GestaoEventosWorkshops.Data;
using GestaoEventosWorkshops.Repositories;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Configuracao da string de conexao para MySQL, obtida do appsettings.json
var connectionString =
	Environment.GetEnvironmentVariable("ConnectionStrings__ConexaoPadrao");

Console.WriteLine($"CONNECTION STRING: {connectionString}");

if (string.IsNullOrEmpty(connectionString))
{
	throw new Exception("VARIAVEL NAO ENCONTRADA");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuracao do Swagger para documentacao da API
builder.Services.AddSwaggerGen(c =>
{
    // Configuracao de seguranca para JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe apenas o token JWT gerado no login."
    });

    // Requer autenticacao para acessar os endpoints no Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Configuracao do DbContext para MySQL usando Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

// Registrando os repositÃ³rios e serviÃ§os para injeÃ§Ã£o de dependÃªncia
builder.Services.AddScoped<IParticipanteRepository, ParticipanteRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IWorkshopRepository, WorkshopRepository>();
builder.Services.AddScoped<IInscricaoRepository, InscricaoRepository>();
builder.Services.AddScoped<IParticipanteService, ParticipanteService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IWorkshopService, WorkshopService>();
builder.Services.AddScoped<IInscricaoService, InscricaoService>();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key nao configurada.");
var key = Encoding.ASCII.GetBytes(jwtKey);

// Configuracao da autenticacao JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
        };
    });

// Configuracao de autorizacao com base em roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SomenteAdministrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("EquipeEventos", policy => policy.RequireRole("Administrador", "Organizador"));
});

// Configuracao de CORS para permitir acesso da interface web
builder.Services.AddCors(options =>
{
    options.AddPolicy("InterfaceWeb", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Habilitando a interface web em wwwroot (index.html, app.js, styles.css)
app.UseDefaultFiles();
app.UseStaticFiles();

// Habilitando CORS para a interface web
app.UseCors("InterfaceWeb");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

