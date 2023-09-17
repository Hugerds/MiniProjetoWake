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
                var resultado = repository.BuscaPorDataAtualizacao(dataAtualizacao);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult BuscaTodos()
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resultado = repository.BuscaTodos();
                return Ok(resultado);
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
                var resultado = await repository.CriaOuEdita(entity);
                return Ok(resultado);
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
                if (!entity.ValidaCampos(out var result)) return BadRequest(result);
                var resultado = await repository.Cria(entity);
                return Ok(resultado);
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
                var resultado = await repository.Edita(entity);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BuscarComFiltroOrdenacao")]
        public IActionResult BuscarComFiltroOrdenacao(string ordenarPor, bool ascendente, string? filtrarPor = null, string? valorFiltro = null)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resultado = repository.BuscarComFiltroOrdenacao(ordenarPor, ascendente, filtrarPor, valorFiltro);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Deleta(Guid id)
        {
            try
            {
                var repository = _scopeFactory.CreateScope().ServiceProvider.GetService<TRepository>() ?? throw new Exception();
                var resultado = await repository.Deleta(id);
                if (!resultado) throw new Exception();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
