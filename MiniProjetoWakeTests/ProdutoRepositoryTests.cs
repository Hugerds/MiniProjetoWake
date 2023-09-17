using Microsoft.Extensions.DependencyInjection;
using MiniProjetoWakeCore.Data.Models;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;

namespace MiniProjetoWakeTests
{
    public class ProdutoRepositoryTests
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoRepositoryTests()
        {
            var startup = new TestStartup();
            var serviceProvider = startup.ConfigureServices(new ServiceCollection());
            _produtoRepository = serviceProvider.GetRequiredService<IProdutoRepository>();
        }

        [Fact]
        public async Task DeveCriarProduto()
        {
            // Arrange
            var produto = new Produto { Nome = "ProdutoTeste", Estoque = 10, Valor = 20 };

            // Act
            var resultado = await _produtoRepository.Cria(produto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(produto.Nome, resultado.Nome);
            // Adicione mais verificações conforme necessário.
        }

        [Fact]
        public async Task DeveAtualizarProduto()
        {
            // Arrange
            var produto = new Produto { Nome = "ProdutoTeste", Estoque = 10, Valor = 20 };
            await _produtoRepository.Cria(produto);

            // Atualize os dados do produto
            produto.Nome = "ProdutoAtualizado";
            produto.Estoque = 5;

            // Act
            var resultado = await _produtoRepository.Edita(produto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(produto.Nome, resultado.Nome);
            // Adicione mais verificações conforme necessário.
        }

        [Fact]
        public async Task DeveDeletarProduto()
        {
            // Arrange
            var produto = new Produto { Nome = "ProdutoTeste", Estoque = 10, Valor = 20 };
            await _produtoRepository.Cria(produto);

            // Act
            var resultado = await _produtoRepository.Deleta(produto.Id);

            // Assert
            Assert.True(resultado);
            Produto? produtoDeletado;
            try
            {
                produtoDeletado = await _produtoRepository.BuscaPorId(produto.Id);
            }
            catch (Exception)
            {
                produtoDeletado = null;
            }
            Assert.Null(produtoDeletado); // O produto não deve ser encontrado após a exclusão.
        }

        [Fact]
        public async Task DeveListarProdutos()
        {
            // Arrange: Adicione 5 produtos à base de dados de teste.
            var produtosParaAdicionar = new List<Produto>
            {
                new Produto { Nome = "Produto 1", Estoque = 10, Valor = 10 },
                new Produto { Nome = "Produto 2", Estoque = 20, Valor = 20 },
                new Produto { Nome = "Produto 3", Estoque = 15, Valor = 30 },
                new Produto { Nome = "Produto 4", Estoque = 5, Valor = 40 },
                new Produto { Nome = "Produto 5", Estoque = 25, Valor = 50 }
            };
            foreach (var produto in produtosParaAdicionar)
            {
                await _produtoRepository.Cria(produto);
            }

            // Act
            var produtos = _produtoRepository.BuscaTodos();

            // Assert
            Assert.Equal(5, produtos.Count());
            // Adicione mais verificações conforme necessário.
        }

        [Fact]
        public async Task DeveBuscarProdutoPorNome()
        {
            // Arrange: Adicione produtos à base de dados de teste.
            var produto = new Produto { Nome = "ProdutoTeste", Estoque = 10, Valor = 20 };
            await _produtoRepository.Cria(produto);

            // Act
            var produtosEncontrados = _produtoRepository.BuscarComFiltroOrdenacao("Nome", true, "Nome", "ProdutoTeste");

            // Assert
            Assert.NotEmpty(produtosEncontrados);
            Assert.All(produtosEncontrados, produto => Assert.Contains("ProdutoTeste", produto.Nome, StringComparison.OrdinalIgnoreCase));
        }
    }
}
