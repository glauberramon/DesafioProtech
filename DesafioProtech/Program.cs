using Microsoft.EntityFrameworkCore;
using DesafioProtech.Infrastructure;
using DesafioProtech.Domain.Interfaces;
using DesafioProtech.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add InMemory Database
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AnimesDB"));

builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
//

// Add Documentation Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Animes ProTech",
        Version = "v1",
        Description = "API para gerenciamento de catálogo de animes",
        Contact = new OpenApiContact
        {
            Name = "Glauber Ramon",
            Email = "glauber.ramon@gmail.com"
        }
    });

    // Documentação do projeto API
    var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
    c.IncludeXmlComments(apiXmlPath);

    // Documentação do projeto Domain
    var domainAssembly = typeof(DesafioProtech.Domain.Entities.Anime).Assembly; // Ajuste o namespace conforme necessário
    var domainXmlFile = $"{domainAssembly.GetName().Name}.xml";
    var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);

    if (File.Exists(domainXmlPath))
    {
        c.IncludeXmlComments(domainXmlPath);
    }
    else
    {
        // Log opcional para ajudar no debug
        Console.WriteLine($"Arquivo de documentação do Domain não encontrado em: {domainXmlPath}");
    }
});
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Animes v1");
        c.DefaultModelsExpandDepth(-1); // Oculta schemas por padrão
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
