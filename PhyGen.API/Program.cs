using PhyGen.Insfrastructure.Extensions;
using PhyGen.Insfrastructure.Persistence.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
