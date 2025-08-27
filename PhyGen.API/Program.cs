using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhyGen.API.Mapping;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Exams.Interfaces;
using PhyGen.Infrastructure.Extensions;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using PhyGen.Infrastructure.Service;
using PhyGen.Infrastructure.Service.Export;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddDatabase<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidDataException("The DefaultConnection string is missing in the configuration."));

builder.Services.Configure<PhyGen.Application.PayOs.Config.PayOSConfig>(builder.Configuration.GetSection("PayOS"));
builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(typeof(ModelMappingProfile));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var secretKey = Encoding.UTF8.GetBytes(jwtSettings.Secret);
builder.Services.AddSingleton(sp =>
{
    var cfg = builder.Configuration.GetSection("Cloudinary");
    var account = new Account(cfg["CloudName"], cfg["ApiKey"], cfg["ApiSecret"]);
    return new Cloudinary(account) { Api = { Secure = true } };
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = 401,
                Message = "Bạn chưa đăng nhập hoặc token không hợp lệ."
            });
            return context.Response.WriteAsync(result);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = 403,
                Message = "Bạn không có quyền truy cập."
            });
            return context.Response.WriteAsync(result);
        }
    };
});

var timeoutSeconds = builder.Configuration.GetValue<int?>("LatexMathmlService:TimeoutSeconds") ?? 5;
builder.Services
    .AddHttpClient<ILatexConvertService, LatexConvertService>(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    });

builder.Services.AddSingleton<IMathmlToOmmlService, MathmlToOmmlService>();
builder.Services.AddScoped<IFormulaConvertPipeline, FormulaConvertPipeline>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IExamExportService, ExamExportService>();

builder.Services.AddHostedService<NodeMathmlHostedService>();

var app = builder.Build();

// Migrate and seed database
await app.MigrateDatabaseAsync<AppDbContext>();
await app.SeedAuthDataAsync();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
