using Microsoft.AspNetCore.Mvc;
using MiniProjetoWakeCore.Data.Models;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using MiniProjetoWakeWEB.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MiniProjetoWakeWEB.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public ProdutoController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var produtoRepository = serviceProvider.GetService<IProdutoRepository>() ?? throw new Exception("Injeção do repositório de produtos não encontrada");
                var produtos = produtoRepository.BuscaTodos().ToList();
                return View(produtos);
            }
        }
        public async Task<IActionResult> Edita([FromBody] ProdutoViewModel produtoViewModel)
        {
            using var scope = _scopeFactory.CreateScope();
            string valorLimpo = produtoViewModel.Valor.Replace("R$ ", "").Replace(".", "");
            decimal valorDecimal = decimal.Parse(valorLimpo, new CultureInfo("pt-BR"));
            var serviceProvider = scope.ServiceProvider;
            var produtoRepository = serviceProvider.GetService<IProdutoRepository>() ?? throw new Exception("Injeção do repositório de produtos não encontrada");
            var produto = await produtoRepository.BuscaPorId(produtoViewModel.Id);
            produto.Valor = valorDecimal;
            produto.Nome = produtoViewModel.Nome;
            produto.Estoque = produtoViewModel.Estoque;
            produto = await produtoRepository.Edita(produto);
            return Ok(true);
        }
        [HttpPost]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                var produtoRepository = serviceProvider.GetService<IProdutoRepository>() ?? throw new Exception("Injeção do repositório de produtos não encontrada");
                var produto = await produtoRepository.BuscaPorId(id);
                produto.Excluido = true;
                await produtoRepository.Edita(produto);
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }
        [HttpPatch]
        public async Task<IActionResult> CriaOuEdita([FromBody] ProdutoViewModel produtoViewModel)
        {
            using var scope = _scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var produtoRepository = serviceProvider.GetService<IProdutoRepository>() ?? throw new Exception("Injeção do repositório de produtos não encontrada");
            var produto = new Produto
            {
                Id = produtoViewModel.Edicao ? produtoViewModel.Id : Guid.NewGuid(),
                Nome = produtoViewModel.Nome,
                Estoque = produtoViewModel.Estoque,
                Valor = decimal.Parse(produtoViewModel.Valor.Replace("R$ ", "").Replace(".", ""), new CultureInfo("pt-BR"))
            };
            if (!produto.ValidaProduto(out ICollection<ValidationResult> results))
            {
                var resultadoComJoinString = string.Join("\n", results.Select(x => x.ErrorMessage));
                return BadRequest("Erro: " + resultadoComJoinString);
            }
            // Verifica se já existe produto com o mesmo nome
            var produtoExistente = produtoRepository.BuscarComFiltroOrdenacao("Nome", true, "Nome", produto.Nome).FirstOrDefault();
            if (produtoExistente != null && ((produtoViewModel.Edicao && produtoExistente.Id != produtoViewModel.Id) || !produtoViewModel.Edicao))
            {
                return BadRequest("Erro: Já existe um produto com esse nome");
            }
            produto = await produtoRepository.CriaOuEdita(produto);
            return Ok(true);
        }
    }
}
