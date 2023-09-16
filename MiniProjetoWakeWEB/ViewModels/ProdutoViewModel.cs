namespace MiniProjetoWakeWEB.ViewModels
{
    public class ProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Estoque { get; set; }
        public string Valor { get; set; } = string.Empty;
        public bool Edicao { get; set; }
    }

}
