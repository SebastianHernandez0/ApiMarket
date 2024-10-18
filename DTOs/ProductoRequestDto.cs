namespace ApiMarket.DTOs
{
    public class ProductoRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public int CategoryId { get; set; }
    }
}
