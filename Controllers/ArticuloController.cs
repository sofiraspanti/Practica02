using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica02.Datos.Interfaces;
using Practica02.Models;
using Practica02.Servicios;

namespace Practica02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly ArticuloServicio _articuloServicio;

        public ArticuloController(ArticuloServicio articuloServicio)
        {
            _articuloServicio = articuloServicio ?? throw new ArgumentNullException(nameof(articuloServicio));
        }



        [HttpPost] // Indica que este método POST crea nuevos recursos --- FUNCIONA OK!
        public IActionResult Create([FromBody] Articulo articulo)
        {
            if (articulo == null)
            {
                return BadRequest("El articulo es nulo");
            }

            bool result = _articuloServicio.AddArticulo(articulo);

            if (result)
            {
                return CreatedAtAction(nameof(_articuloServicio.GetById), new { id = articulo.id_art }, articulo);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el articulo");
            }
        }

        [HttpGet] // para obtener toda la lista de articulos --- FUNCIONA OK!
        public IActionResult GetAll()
        {
            var articulos = _articuloServicio.GetAllArticulo();

            if (articulos == null || !articulos.Any())
            {
                return NotFound("La lista de articulos esta vacía");
            }

            return Ok(articulos);
        }

        [HttpGet("{id}")]  // FUNCIONA OK!
        public IActionResult GetById(int id)
        {
            var articulo = _articuloServicio.GetById(id);

            if (articulo == null)
            {
                return NotFound("No se encuantra el articulo");
            }

            return Ok(articulo);
        }

        [HttpPut("{id}")] /// metodo para modificar -- FUNCIONA OK!
        public IActionResult Put(int id, string nombre, decimal precio, [FromBody] Articulo articulo)
        {
            if (articulo == null)
            {
                return BadRequest("El articulo no puede ser nulo");
            }

            var articuloExiste = _articuloServicio.GetById(id);
            if (articuloExiste == null)
            {
                return NotFound("El articulo no existe.");
            }

            bool result = _articuloServicio.UpdateArticulo(id, nombre, precio);

            if (result)
            {
                return NoContent(); // se realizo la actualizacion
            } else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar");
            }
                                        
        }
        [HttpDelete("{id}")] // FUNCIONA OK!
        public IActionResult Delete(int id)
        {
            bool result = _articuloServicio.DeteleArticulo(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
