using EshoppingZone.Data;
using EshoppingZone.Models;
using EshoppingZone.Repositories.Implementations;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Services.Implementations;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// ADD: for ProblemDetails + status codes + JSON
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder();

// EF Core SQL Server
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// MVC / API plumbing
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Enter JWT token",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await DbSeeder.SeedAdminAsync(userManager);
}

/*  Global Exception-Handling Middleware  */
/* This does NOT change successful outputs. It only shapes unhandled errors.       */
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Minimal logging (you can swap to ILogger/Serilog later)
        Console.Error.WriteLine($"[Unhandled] {DateTimeOffset.UtcNow:o} {context.Request?.Method} {context.Request?.Path} :: {ex}");

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Type    = "about:blank",
            Title   = "An unexpected error occurred.",
            Status  = StatusCodes.Status500InternalServerError,
            Detail  = app.Environment.IsDevelopment() ? ex.Message : null,  // hide details in Prod
            Instance = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        // short-circuits the pipeline since the response is written
    }
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Map attribute-routed controllers
app.MapControllers();

app.Run();