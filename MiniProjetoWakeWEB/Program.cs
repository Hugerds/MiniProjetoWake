using Microsoft.EntityFrameworkCore;
using MiniProjetoWakeCore;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using MiniProjetoWakeCore.Data.Repositories;
using ControleEstoqueAmostrasAPI.Services.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services
                .AddDbContext<MiniProjetoWakeContext>(
                    options => options.UseSqlite(
                        builder.Configuration.GetConnectionString("MiniProjetoWakeWEBConnection")!));

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddHostedService<CriacaoProdutoService>();
var app = builder.Build();

try
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MiniProjetoWakeContext>();
    dbContext.Database.Migrate();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});
app.MapRazorPages();

app.Run();