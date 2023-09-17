using MiniProjetoWakeCore.Data.Models.Base;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjetoWakeTests.Repositories
{
    public class MockBaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly List<T> _data;

        public MockBaseRepository()
        {
            _data = new List<T>();
        }

        public IEnumerable<T> BuscaTodos()
        {
            return _data.Where(x => !x.Excluido).ToList();
        }

        public async Task<T> BuscaPorId(Guid id)
        {
            var model = _data.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(model) ?? throw new Exception("Item não encontrado");
        }

        public async Task<T> Cria(T model)
        {
            model.Id = Guid.NewGuid();
            _data.Add(model);
            return await BuscaPorId(model.Id);
        }

        public async Task<T> CriaOuEdita(T model)
        {
            var existingModel = _data.FirstOrDefault(x => x.Id == model.Id);
            if (existingModel == null)
            {
                model.Id = Guid.NewGuid();
                _data.Add(model);
            }
            else
            {
                _data.Remove(existingModel);
                _data.Add(model);
            }
            return await BuscaPorId(model.Id);
        }

        public async Task<bool> Deleta(Guid id)
        {
            var entity = _data.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                _data.Remove(entity);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public IEnumerable<T> BuscaPorDataAtualizacao(DateTime? dataAtualizacao)
        {
            return _data.Where(x => dataAtualizacao == null ? !x.Excluido : x.DataAtualizacao > dataAtualizacao).ToList();
        }

        public async Task<T> Edita(T model)
        {
            var existingModel = _data.FirstOrDefault(x => x.Id == model.Id);
            if (existingModel == null)
            {
                throw new Exception("Item não encontrado");
            }
            _data.Remove(existingModel);
            _data.Add(model);
            return await BuscaPorId(model.Id);
        }

        public List<string> BuscaCamposClasse()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToList();
        }

        public IEnumerable<T> BuscarComFiltroOrdenacao(string ordenarPor, bool ascendente, string? filtrarPor = null, string? valorFiltro = null)
        {
            var camposValidos = BuscaCamposClasse();
            if (string.IsNullOrEmpty(ordenarPor) || !camposValidos.Contains(ordenarPor))
            {
                throw new Exception("Campo de ordenação inválido");
            }

            var itens = BuscaTodos().AsQueryable();
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, ordenarPor);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            if (!string.IsNullOrEmpty(filtrarPor) && valorFiltro != null)
            {
                var parameterFiltro = Expression.Parameter(typeof(T), "x");
                var propertyFiltro = Expression.Property(parameterFiltro, filtrarPor);
                var constant = Expression.Constant(valorFiltro);
                var equal = Expression.Equal(propertyFiltro, constant);
                var lambdaFiltro = Expression.Lambda<Func<T, bool>>(equal, parameterFiltro);
                itens = itens.Where(lambdaFiltro);
            }

            return ascendente ? itens.OrderBy(lambda).ToList() : itens.OrderByDescending(lambda).ToList();
        }
    }

}
