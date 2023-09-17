using ControleEstoqueAmostrasAPI.Services.BackgroundServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiniProjetoWakeCore;
using MiniProjetoWakeCore.Data.Repositories;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services
                .AddDbContext<MiniProjetoWakeContext>(
                    options => options.UseSqlite(
                        builder.Configuration.GetConnectionString("MiniProjetoWakeAPIConnection")!));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mini Projeto Wake",
        Version = "1.0.0",
        Description = "Documentação - Mini Projeto Wake",
    });
});

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddHostedService<CriacaoProdutoService>();
var app = builder.Build();

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<MiniProjetoWakeContext>();
dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<AuthorizationMiddleware>();

app.MapControllers();

app.Run();
public partial class Program { }