using ApiMarket.DTOs;
using ApiMarket.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Context _context;
        private IValidator<ProductoRequestDto> _productInsertValidator;

        public ProductsController(Context context, IValidator<ProductoRequestDto> productoInsertValidator)
        {
            _context = context;
            _productInsertValidator = productoInsertValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductos()
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

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductoResponseDto>> GetById(int id)
        {
            var producto = await _context.Productos.Include(p => p.Usuario).Include(p => p.ProductosCategorias).ThenInclude(pc => pc.Categoria).FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }
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

            return Ok(productoDto);
        }

        [HttpPost]

        public async Task<ActionResult<ProductoResponseDto>> Add(ProductoRequestDto productoRequestDto)
        {
            var validationResult = _productInsertValidator.Validate(productoRequestDto);
            if (!validationResult.IsValid) {
                return BadRequest(validationResult.Errors);
            }

            var productoExistente = await _context.Productos.FirstOrDefaultAsync(p => p.Name.ToLower() == productoRequestDto.Name.ToLower());
            if (productoExistente != null)
            {
                return BadRequest("El producto ya existe");
            }

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

            return Ok (Producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoResponseDto>> Update(int id, ProductoRequestDto productoRequestDto)
        {
            var producto = await _context.Productos.Include(p => p.Usuario).Include(p => p.ProductosCategorias).ThenInclude(pc => pc.Categoria).FirstOrDefaultAsync(p => p.Id == id); ;

            if (producto == null)
            {
                return NotFound();
            }

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
            return Ok(ProductoDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
