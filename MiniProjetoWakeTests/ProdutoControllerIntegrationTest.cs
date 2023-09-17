using Microsoft.AspNetCore.Mvc.Testing;
using MiniProjetoWakeCore.Data.Models;
using System.Net;
using System.Net.Http.Json;

namespace MiniProjetoWakeTests
{
    public class ProdutoControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProdutoControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task DeveRetornarOKAoListarProdutos()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Produto"); // Suponha que este seja o endpoint para listar produtos

            // Assert
            response.EnsureSuccessStatusCode(); // Verifique se a resposta tem um status de sucesso (HTTP 200)
        }

        [Fact]
        public async Task DeveRetornarVazioAoBuscarProdutoInexistente()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var ordenarPor = "Nome";
            var ascendente = true;
            var filtrarPor = "Nome";
            var valorFiltro = "ProdutoInexistente";

            // Crie a URL com os parâmetros de consulta
            var url = $"/api/Produto/BuscarComFiltroOrdenacao?ordenarPor={ordenarPor}&ascendente={ascendente}&filtrarPor={filtrarPor}&valorFiltro={valorFiltro}";

            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Verifique se a resposta tem um status de sucesso (HTTP 200)

            // Verifique se o corpo da resposta está vazio
            var content = await response.Content.ReadFromJsonAsync<List<Produto>>();
            Assert.NotNull(content);
            Assert.Empty(content);
        }

        [Fact]
        public async Task DeveRetornarProdutoAoBuscarProdutoExistente()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Crie um produto para testar
            var produto = new Produto
            {
                Nome = "ProdutoTeste",
                Valor = 10,
                Estoque = 1,
            };
            await client.PostAsJsonAsync("/api/Produto", produto);

            // Act
            var ordenarPor = "Nome";
            var ascendente = true;
            var filtrarPor = "Nome";
            var valorFiltro = "ProdutoTeste";
            // Crie a URL com os parâmetros de consulta
            var url = $"/api/Produto/BuscarComFiltroOrdenacao?ordenarPor={ordenarPor}&ascendente={ascendente}&filtrarPor={filtrarPor}&valorFiltro={valorFiltro}";
            var response = await client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode(); // Verifique se a resposta tem um status de sucesso (HTTP 200)
            // Verifique se o corpo da resposta está válido, deve ser um tipo List<Produto>
            var content = await response.Content.ReadFromJsonAsync<List<Produto>>();
            Assert.NotNull(content);
            Assert.NotEmpty(content);

            // Deleta Produto
            await client.DeleteAsync($"/api/Produto?id={produto.Id}");
        }

        [Theory]
        [InlineData("Produto Teste", 10, 1)]
        [InlineData("Produto Teste 2", 20, 2)]
        [InlineData("Produto Teste 3", 30, 3)]
        public async Task DeveRetornarOKAoCriarProdutoComDadosCorretos(string nome, decimal valor, int estoque)
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var produto = new Produto
            {
                Nome = nome,
                Valor = valor,
                Estoque = estoque,
            };
            var response = await client.PostAsJsonAsync("/api/Produto", produto);
            // Assert
            response.EnsureSuccessStatusCode(); // Verifique se a resposta tem um status de sucesso (HTTP 200)
            // Verifique se o corpo da resposta está válido, deve ser um tipo Produto
            var content = await response.Content.ReadFromJsonAsync<Produto>();
            Assert.NotNull(content);
        }

        [Fact]
        public async Task DeveRetornarBadRequestAoCriarProdutoComValorNegativo()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var produto = new Produto
            {
                Nome = "Produto Teste",
                Valor = -10,
                Estoque = 1,
            };
            var response = await client.PostAsJsonAsync("/api/Produto", produto);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Deleta Produto
            await client.DeleteAsync($"/api/Produto?id={content.Id}");
        }

        [Fact]
        public async Task DeveRetornarBadRequestAoCriarProdutoComNomeVazio()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var produto = new Produto
            {
                Nome = "",
                Valor = 10,
                Estoque = 1,
            };
            var response = await client.PostAsJsonAsync("/api/Produto", produto);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornarBadRequestAoDeletarProdutoInexistente()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.DeleteAsync($"/api/Produto?id={Guid.NewGuid()}");
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornarOKAoDeletarProdutoExistente()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var produto = new Produto
            {
                Nome = "Produto Teste",
                Valor = 10,
                Estoque = 1,
            };
            var response = await client.PostAsJsonAsync("/api/Produto", produto);
            var produtoCriado = await response.Content.ReadFromJsonAsync<Produto>();

            Assert.NotNull(produtoCriado);

            var responseDelete = await client.DeleteAsync($"/api/Produto?id={produtoCriado.Id}");
            // Assert
            responseDelete.EnsureSuccessStatusCode(); // Verifique se a resposta tem um status de sucesso (HTTP 200)
        }

        [Fact]
        public async Task DeveRetornarBadRequestAoAtualizarProdutoInexistente()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Teste",
                Valor = 10,
                Estoque = 1,
            };
            var response = await client.PutAsJsonAsync("/api/Produto", produto);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornarOKAoAtualizarProdutoExistente()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var produto = new Produto
            {
                Nome = "Produto Teste",
                Valor = 10,
                Estoque = 1,
            };
            var response = await client.PostAsJsonAsync("/api/Produto", produto);
            var produtoCriado = await response.Content.ReadFromJsonAsync<Produto>();
            Assert.NotNull(produtoCriado);
            produtoCriado.Nome = "Produto Teste Editado";
            produtoCriado.Valor = 20;
            produtoCriado.Estoque = 2;
            var responsePut = await client.PutAsJsonAsync("/api/Produto", produtoCriado);
            var produtoEditado = await responsePut.Content.ReadFromJsonAsync<Produto>();
            Assert.NotNull(produtoEditado);
            // Assert
            responsePut.EnsureSuccessStatusCode(); // Verifique se a resposta tem um status de sucesso (HTTP 200)
            Assert.Equal(produtoCriado.Nome, produtoEditado.Nome);
            Assert.Equal(produtoCriado.Valor, produtoEditado.Valor);
            Assert.Equal(produtoCriado.Estoque, produtoEditado.Estoque);

            // Deleta Produto
            await client.DeleteAsync($"/api/Produto?id={produtoEditado.Id}");
        }
    }

}
