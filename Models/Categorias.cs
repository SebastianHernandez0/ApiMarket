using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMarket.Models
{
    public class Categorias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ProductosCategorias> ProductosCategorias { get; set; } = new List<ProductosCategorias>();

    }
}
