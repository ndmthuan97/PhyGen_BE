using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PhyGen.API.Mapping;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Extensions;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Insfrastructure.Persistence.Repositories;
using System.Security.Claims;
using System.Text;

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

builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(typeof(ModelMappingProfile));

var app = builder.Build();

// Migrate and seed database
await app.MigrateDatabaseAsync<AppDbContext>();
await app.SeedAuthDataAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
