using Microsoft.EntityFrameworkCore;
using TestProjectApi.Entities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<TodoContext> (opt => opt.UseSqlServer(
    // TODO à config
    builder.Configuration.GetConnectionString("DefaultConnection")  ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(option =>
    {
        option.DocumentPath = "/openapi/v1.json";
        
    });
}

/*if (app.Environment.IsProduction())
{
    
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
