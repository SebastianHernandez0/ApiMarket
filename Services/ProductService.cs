using ApiMarket.DTOs;
using ApiMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMarket.Services
{
    public class ProductService : ICommonService<ProductoResponseDto, ProductoRequestDto, ProductoRequestDto>
    {
        private readonly Context _context;

        public ProductService(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoResponseDto>> Get()
        {
            var productos = await _context.Productos.Include(p => p.Usuario).Include(p => p.ProductosCategorias).ThenInclude(pc => pc.Categoria).ToListAsync();
            var productoResponseDtos = productos.Select(p => new ProductoResponseDto


            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Image = p.Image,

                Usuario = new UsuarioResponseDto
                {
                    UsuarioId = p.Usuario.UsuarioId,
                    Name = p.Usuario.Name,
                    Email = p.Usuario.Email
                },

                Category = new CategoriaResponseDto
                {
                    Id = p.ProductosCategorias.Select(pc => pc.Categoria.CategoryId).FirstOrDefault(),
                    Name = p.ProductosCategorias.Select(pc => pc.Categoria.Name).FirstOrDefault()
                }




            }).ToList();


            return productoResponseDtos;
        }

        public async Task<ProductoResponseDto> GetById(int id)
        {
            var producto = await _context.Productos.Include(p => p.Usuario).Include(p => p.ProductosCategorias).ThenInclude(pc => pc.Categoria).FirstOrDefaultAsync(p => p.Id == id);

            if (producto != null)
            {
                var productoDto = new ProductoResponseDto
                {
                    Id = producto.Id,
                    Name = producto.Name,
                    Description = producto.Description,
                    Price = producto.Price,
                    Stock = producto.Stock,
                    Image = producto.Image,

                    Usuario = new UsuarioResponseDto
                    {
                        UsuarioId = producto.Usuario.UsuarioId,
                        Name = producto.Usuario.Name,
                        Email = producto.Usuario.Email
                    },

                    Category = new CategoriaResponseDto
                    {
                        Id = producto.ProductosCategorias.Select(pc => pc.Categoria.CategoryId).FirstOrDefault(),
                        Name = producto.ProductosCategorias.Select(pc => pc.Categoria.Name).FirstOrDefault()
                    }
                 
                };

                return productoDto;
            }

            return null;

        }

        public async Task<ProductoResponseDto> Add(ProductoRequestDto productoRequestDto)
        {
            var Producto = new Productos
            {
                Name = productoRequestDto.Name,
                Description = productoRequestDto.Description,
                Price = productoRequestDto.Price,
                Stock = productoRequestDto.Stock,
                Image = productoRequestDto.Image,
                UsuarioId = productoRequestDto.UsuarioId,
            };

            await _context.Productos.AddAsync(Producto);
            await _context.SaveChangesAsync();

            var ProductoCategoria = new ProductosCategorias
            {
                ProductoId = Producto.Id,
                CategoriaId = productoRequestDto.CategoryId
            };

            await _context.ProductosCategorias.AddAsync(ProductoCategoria);
            await _context.SaveChangesAsync();

            var ProductoDto= new ProductoResponseDto
            {
                    Id = Producto.Id,
                    Name = Producto.Name,
                    Description = Producto.Description,
                    Price = Producto.Price,
                    Stock = Producto.Stock,
                    Image = Producto.Image,
            };
            return ProductoDto;
        }
        public async Task<ProductoResponseDto> Update(int id, ProductoRequestDto productoRequestDto)
        {
            var producto = await _context.Productos.Include(p => p.Usuario).Include(p => p.ProductosCategorias).ThenInclude(pc => pc.Categoria).FirstOrDefaultAsync(p => p.Id == id); ;

            if (producto != null)
            {
                producto.Name = productoRequestDto.Name;
                producto.Description = productoRequestDto.Description;
                producto.Price = productoRequestDto.Price;
                producto.Stock = productoRequestDto.Stock;
                producto.Image = productoRequestDto.Image;
                producto.ProductosCategorias = new List<ProductosCategorias>
            {
                new ProductosCategorias
                {
                    CategoriaId = productoRequestDto.CategoryId
                }
            };

                await _context.SaveChangesAsync();

                var ProductoDto = new ProductoResponseDto
                {
                    Id = producto.Id,
                    Name = producto.Name,
                    Description = producto.Description,
                    Price = producto.Price,
                    Stock = producto.Stock,
                    Image = producto.Image,

                    Usuario = new UsuarioResponseDto
                    {
                        UsuarioId = producto.Usuario.UsuarioId,
                        Name = producto.Usuario.Name,
                        Email = producto.Usuario.Email
                    },

                    Category = new CategoriaResponseDto
                    {
                        Id = producto.ProductosCategorias.Select(pc => pc.Categoria.CategoryId).FirstOrDefault(),
                        Name = producto.ProductosCategorias.Select(pc => pc.Categoria.Name).FirstOrDefault()
                    }

                };
                return ProductoDto;
            }

            return null;
            
        }

        public async Task Delete(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            
            
        }
    }
}
