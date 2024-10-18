using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMarket.Models
{
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; }= string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public virtual ICollection<Productos> Productos { get; set; }= new List<Productos>();
    }
}
