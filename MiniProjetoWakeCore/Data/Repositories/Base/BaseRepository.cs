using AutoMapper;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using MiniProjetoWakeCore.Data.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MiniProjetoWakeCore;
using Microsoft.IdentityModel.Tokens;

namespace MiniProjetoWakeCore.Data.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        public readonly MiniProjetoWakeContext _context;
        public BaseRepository(MiniProjetoWakeContext context)
        {
            _context = context;
        }
        public virtual IEnumerable<T> BuscaTodos()
        {
            return _context.Set<T>().AsNoTracking().Where(x => !x.Excluido);
        }
        public async Task<T> BuscaPorId(Guid id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Item não encontrado");
        }
        public async Task<T> Cria(T model)
        {
            try
            {
                await _context.Set<T>().AddAsync(model);
                await _context.SaveChangesAsync();
                return await BuscaPorId(model.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<T> CriaOuEdita(T model)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<T, T>();
                }).CreateMapper();
                var modelNoBanco = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == model.Id);
                if (modelNoBanco == null)
                {
                    modelNoBanco = mapper.Map<T>(model);
                    await _context.Set<T>().AddAsync(modelNoBanco);
                }
                else
                {
                    modelNoBanco = mapper.Map(model, modelNoBanco);
                    _context.Set<T>().Update(modelNoBanco).Property(x => x.Codigo).IsModified = false;
                }
                await _context.SaveChangesAsync();
                return await BuscaPorId(model.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Deleta(Guid id)
        {
            try
            {
                var model = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Item não encontrado");
                _context.Set<T>().Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<T> BuscaPorDataAtualizacao(DateTime? dataAtualizacao)
        {
            return _context.Set<T>().AsNoTracking().Where(x => dataAtualizacao == null ? !x.Excluido : x.DataAtualizacao > dataAtualizacao);
        }

        public async Task<T> Edita(T model)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<T, T>();
                }).CreateMapper();
                var modelNoBanco = _context.Set<T>().FirstOrDefault(x => x.Id == model.Id);
                if (modelNoBanco == null)
                {
                    throw new Exception();
                }
                modelNoBanco = mapper.Map(model, modelNoBanco);
                _context.Set<T>().Update(modelNoBanco).Property(x => x.Codigo).IsModified = false;
                _context.SaveChanges();
                return await BuscaPorId(model.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> BuscaCamposClasse()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToList();
        }
        /// <summary>
        /// Realiza uma busca com filtro e ordenação dinâmicos.
        /// </summary>
        /// <param name="ordenarPor">Campo para ordenação.</param>
        /// <param name="ascendente">Indica se a ordenação deve ser ascendente.</param>
        /// <param name="filtrarPor">Campo para filtro.</param>
        /// <param name="valorFiltro">Valor para filtro.</param>
        /// <returns>Uma coleção de entidades ordenada e filtrada.</returns>
        public IEnumerable<T> BuscarComFiltroOrdenacao(string ordenarPor, bool ascendente, string? filtrarPor = null, string? valorFiltro = null)
        {
            var camposValidos = BuscaCamposClasse();
            // Validação de parâmetros
            if (string.IsNullOrEmpty(ordenarPor) || !camposValidos.Contains(ordenarPor))
            {
                throw new Exception("Campo de ordenação inválido");
            }

            if (!string.IsNullOrEmpty(filtrarPor) && !camposValidos.Contains(filtrarPor))
            {
                throw new Exception("Campo de filtro inválido");
            }

            var itens = BuscaTodos().AsQueryable() ?? throw new Exception("Nenhum item encontrado");

            // Expressão lambda para ordenação
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, ordenarPor);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            // Aplica filtro se necessário
            if (!string.IsNullOrEmpty(filtrarPor) && valorFiltro != null)
            {
                var parameterFiltro = Expression.Parameter(typeof(T), "x");
                var propertyFiltro = Expression.Property(parameterFiltro, filtrarPor);
                var constant = Expression.Constant(valorFiltro);
                var equal = Expression.Equal(propertyFiltro, constant);
                var lambdaFiltro = Expression.Lambda<Func<T, bool>>(equal, parameterFiltro);
                itens = itens.Where(lambdaFiltro);
            }

            // Retorna a lista ordenada
            return ascendente ? itens.OrderBy(lambda) : itens.OrderByDescending(lambda);
        }

    }
}
