using ApiMarket.DTOs;
using ApiMarket.Models;
using ApiMarket.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
        private ICommonService<ProductoResponseDto,ProductoRequestDto,ProductoRequestDto> _commonService;

        public ProductsController(Context context, IValidator<ProductoRequestDto> productoInsertValidator,
            ICommonService<ProductoResponseDto, ProductoRequestDto, ProductoRequestDto> commonService)
        {
            _context = context;
            _productInsertValidator = productoInsertValidator;
            _commonService = commonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductos()
        {
            var productos = await _commonService.Get();
            return Ok(productos);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductoResponseDto>> GetById(int id)
        {
            var producto= await _commonService.GetById(id);

            return producto == null ? NotFound() : Ok(producto);


        }

        [HttpPost]
        [Authorize]

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
            var productoDto= await _commonService.Add(productoRequestDto);

            return CreatedAtAction(nameof(GetById), new { id = productoDto.Id }, productoDto);



        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductoResponseDto>> Update(int id, ProductoRequestDto productoRequestDto)
        {
            var productoDto = await _commonService.Update(id, productoRequestDto);

            return productoDto == null ? NotFound() : Ok(productoDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _commonService.Delete(id);

            return Ok("Producto Eliminado");
        }

    }
}
