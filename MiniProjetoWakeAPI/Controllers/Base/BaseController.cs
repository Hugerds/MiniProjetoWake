using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using MiniProjetoWakeCore.Data.Models.Base;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;

namespace MiniProjetoWakeAPI.Controllers.Base
{
    [Route("api/[controller]")]
    public class BaseController<T, TRepository> : Controller where T : BaseModel where TRepository : IBaseRepository<T>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public BaseController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        [HttpGet("BuscaPorDataAtualizacao")]
        public IActionResult BuscaPorDataAtualizacao(DateTime? dataAtualizacao)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resuiltado = repository.BuscaPorDataAtualizacao(dataAtualizacao);
                return Ok(resuiltado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BuscaTodos")]
        public IActionResult BuscaTodos()
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resuiltado = repository.BuscaTodos();
                return Ok(resuiltado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        public async Task<IActionResult> CriaOuEdita([FromBody] T entity)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resuiltado = await repository.CriaOuEdita(entity);
                return Ok(resuiltado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Cria([FromBody] T entity)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resuiltado = await repository.Cria(entity);
                return Ok(resuiltado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Edita([FromBody] T entity)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resuiltado = await repository.Edita(entity);
                return Ok(resuiltado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BuscarComFiltroOrdenacao")]
        public IActionResult BuscarComFiltroOrdenacao(string ordenarPor, bool ascendente, string? filtrarPor = null, object? valorFiltro = null)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resuiltado = repository.BuscarComFiltroOrdenacao(ordenarPor, ascendente, filtrarPor, valorFiltro);
                return Ok(resuiltado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
