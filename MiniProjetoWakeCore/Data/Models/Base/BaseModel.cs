using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProjetoWakeCore.Data.Models.Base
{
    public class BaseModel
    {
        [Required(ErrorMessage = "O campo Id é obrigatório.")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "O campo DataCriacao é obrigatório.")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "O campo DataAtualizacao é obrigatório.")]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "O campo Excluido é obrigatório.")]
        public bool Excluido { get; set; } = false;
        public bool ValidaCampos(out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(this, context, results, true);
        }
    }
}
