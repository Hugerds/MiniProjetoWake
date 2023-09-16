using MiniProjetoWakeAPI.Controllers.Base;
using MiniProjetoWakeCore;
using MiniProjetoWakeCore.Data.Models;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;

namespace MiniProjetoWakeAPI.Controllers
{
    public class ProdutoController : BaseController<Produto, IProdutoRepository>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public ProdutoController(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
    }
}
