using System.Security.Cryptography;
using Inventario.context;
using Inventario.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductoControllers : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductoControllers> _logger;

    public ProductoControllers(ApplicationDbContext context, ILogger<ProductoControllers> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/inventario/viewAll")]
    public async Task<ActionResult<IEnumerable<Productos>>> viewAll()
    {
        var response = new Dictionary<String, Object>();
        try
        {
            _logger.LogInformation("Se a realizado una consulta");
            response.Add("mensaje", "consulta de productos");
            var productoView = await _context.Productos.ToListAsync();

            return Ok(productoView);

        }
        catch (Exception e)
        {

            _logger.LogInformation(e,"error en la peticion");
            return StatusCode(500, "prueba nuevamente ");
        }
    }

    [HttpPost("/inbentario/newProducto")]
    public async Task<IActionResult> newProducto(Productos productos)
    {
        var response = new Dictionary<string, object>();
        try
        {
            productos.codigoInventario = (int)GenerateRandomInt();
            await _context.Productos.AddAsync(productos);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Se a creado un nuevo producto de inventario");
            response.Add("mensaje", "se a creado un nuevo producto");
            return CreatedAtAction(nameof(viewAll), new { id = productos.codigoInventario });
        }
        catch (Exception e)
        {
        
            _logger.LogInformation(e, "error al crear el producto");
            return StatusCode(500);
        }
    }
    
    private int GenerateRandomInt()
    {
        byte[] buffer = new byte[4]; // 4 bytes para int
        RandomNumberGenerator.Fill(buffer);
        return BitConverter.ToInt32(buffer, 0) & int.MaxValue; // Aseguramos que sea positivo
    }


    [HttpPut("/inventario/updateProducto/{codigoInventario}")]
    public async Task<ActionResult<Dictionary<string, object>>> updateProducto(int codigoInventario,
        [FromBody] Productos productos)
    {
        var response = new Dictionary<string, object>();
        try
        {
            var newProducto = await _context.Productos.FindAsync(codigoInventario);

            if (newProducto == null)
            {
                return NotFound();
            }

            newProducto.Marca = productos.Marca;
            newProducto.TipoEquipo = productos.TipoEquipo;
            newProducto.Departamento = productos.Departamento;
            newProducto.FechaAsignacion = productos.FechaAsignacion;
            newProducto.PersonaResponsable = productos.PersonaResponsable;
            newProducto.FechaIngreso = productos.FechaIngreso;

            await _context.SaveChangesAsync();
            response.Add("mensaje", "Producto actualizado");
            return Ok(response);
        }
        catch (Exception e)
        {

            _logger.LogInformation(e,$"No se a podido realizar la actualizacion de forma correcta...{e.Message}");
            return StatusCode(500);

        }
    }

    [HttpDelete("/inventario/delete/{codigoInventario}")]
    public async Task<ActionResult<Dictionary<string, object>>> deleteProducto(int codigoInventario)
    {
        var response = new Dictionary<string, object>();
        
            var inventario = await _context.Productos.FindAsync(codigoInventario);
            if (inventario == null)
            {
                return NotFound("Invenrario no encontrada");
            }

            _context.Productos.Remove(inventario);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Se elimina el producto {inventario.TipoEquipo}");
                response.Add("mensaje", "se a eliminado el producto");
                return Ok();

            }
        catch (Exception e)
        {

            _logger.LogInformation(e,$"error la eliminacion ... {e.Message}");
            return StatusCode(500);
        }
    }
    
}