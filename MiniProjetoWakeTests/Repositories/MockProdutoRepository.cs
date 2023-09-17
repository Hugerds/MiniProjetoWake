using MiniProjetoWakeCore.Data.Models;
using MiniProjetoWakeCore.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjetoWakeTests.Repositories
{
    public class MockProdutoRepository : MockBaseRepository<Produto>, IProdutoRepository
    {
    }
}
