using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProjetoWakeCore;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using MiniProjetoWakeTests.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjetoWakeTests
{
    public class TestStartup
    {

        public TestStartup()
        {
        }

        public ServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Configurar o contexto do banco de dados em memória para testes
            services.AddDbContext<MiniProjetoWakeContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            // Configurar outros serviços de teste aqui, como mock de repositórios.

            services.AddScoped<IProdutoRepository, MockProdutoRepository>(); // Substitua pelo mock apropriado.
            return services.BuildServiceProvider();
        }

    }

}
