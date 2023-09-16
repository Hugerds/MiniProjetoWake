using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MiniProjetoWakeCore.Data.Models;

namespace MiniProjetoWakeCore
{
    public class MiniProjetoWakeContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public MiniProjetoWakeContext(DbContextOptions<MiniProjetoWakeContext> options) : base(options)
        {
        }
    }
}
