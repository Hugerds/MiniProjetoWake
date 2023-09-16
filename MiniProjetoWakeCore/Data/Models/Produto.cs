using MiniProjetoWakeCore.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MiniProjetoWakeCore.Data.Models
{
    public class Produto : BaseModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O campo Nome deve ter no máximo 255 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Estoque é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O estoque do produto deve ser maior ou igual a zero.")]
        public int Estoque { get; set; }

        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior ou igual a 0.01.")]
        public decimal Valor { get; set; }
        public bool ValidaProduto(out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(this, context, results, true);
        }
    }
}
