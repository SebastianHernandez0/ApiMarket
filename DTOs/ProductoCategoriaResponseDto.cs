namespace ApiMarket.DTOs
{
    public class ProductoCategoriaResponseDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }

        public ProductoResponseDto Producto { get; set; } = new ProductoResponseDto();
        public CategoriaResponseDto Categoria { get; set; } = new CategoriaResponseDto();
    }
}
