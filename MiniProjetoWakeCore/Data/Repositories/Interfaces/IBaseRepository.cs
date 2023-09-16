using MiniProjetoWakeCore.Data.Models.Base;

namespace MiniProjetoWakeCore.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        IEnumerable<T> BuscaTodos();
        Task<T> BuscaPorId(Guid id);
        Task<T> Cria(T model);
        Task<T> CriaOuEdita(T model);
        Task<T> Edita(T model);
        Task<bool> Deletar(Guid id);
        IEnumerable<T> BuscaPorDataAtualizacao(DateTime? dataAtualizacao);
        IEnumerable<T> BuscarComFiltroOrdenacao(string ordenarPor, bool ascendente, string? filtrarPor = null, object? valorFiltro = null);
        List<string> BuscaCamposClasse();
    }
}
