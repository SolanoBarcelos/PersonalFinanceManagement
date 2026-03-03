using API.Middlewares;
using Application.Configuration;
using Infrastructure.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            // Adicionar porta que o front abrir no localhost aqui!
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Personal Finance Management API",
        Version = "v1",
        Description = "API para controle financeiro pessoal utilizando Clean Architecture e DDD."
    });

    void IncludeXml(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, fileName);
        if (File.Exists(path)) c.IncludeXmlComments(path);
    }

    // 1. Projeto API
    var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    IncludeXml(apiXmlFile);

    // 2. Projeto Domain
    IncludeXml("Domain.xml");

    // 3. Projeto Application
    IncludeXml("Application.xml");

    // 4. Projeto Infrastructure
    IncludeXml("Infrastructure.xml");
});

// Delega a responsabilidade de injetar as dependências da camada de Aplicação e Infraestrutura 
// nas próprias camadas, tentando isolar ao máximo as camadas.
// Verificar classe DependencyInjection dentro de Configuration de cada camada.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();
app.MapControllers();

app.Run();