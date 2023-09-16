using MiniProjetoWakeCore.Data.Models;
using MiniProjetoWakeCore.Data.Repositories.Base;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using MiniProjetoWakeCore;

namespace MiniProjetoWakeCore.Data.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(MiniProjetoWakeContext context) : base(context)
        {
        }
    }
}
