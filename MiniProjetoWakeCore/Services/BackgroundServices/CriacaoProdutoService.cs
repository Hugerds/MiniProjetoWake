using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniProjetoWakeCore;
using MiniProjetoWakeCore.Data.Models;

namespace ControleEstoqueAmostrasAPI.Services.BackgroundServices
{
    public class CriacaoProdutoService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CriacaoProdutoService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MiniProjetoWakeContext>() ?? throw new Exception("Instância do banco de dados não encontrada");
            if (!context.Produtos.Any())
            {
                var products = new List<Produto>
                {
                new Produto { Nome = "Produto 1", Estoque = 10, Valor = 10 },
                new Produto { Nome = "Produto 2", Estoque = 20, Valor = 20 },
                new Produto { Nome = "Produto 3", Estoque = 15, Valor = 30 },
                new Produto { Nome = "Produto 4", Estoque = 5, Valor = 40 },
                new Produto { Nome = "Produto 5", Estoque = 25, Valor = 50 },
                };

                await context.Produtos.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }

}
