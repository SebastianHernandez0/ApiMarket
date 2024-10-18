using System.ComponentModel.DataAnnotations;

namespace ApiMarket.DTOs
{
    public class UsuarioLoginDto
    {
        
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
       

    }
}
