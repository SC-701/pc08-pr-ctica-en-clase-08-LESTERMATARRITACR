using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase, ICategoriaController
    {
        private ICategoriaFlujo _categoriaFlujo;
        private ILogger<CategoriaController> _logger;

        public CategoriaController(ICategoriaFlujo categoriaFlujo, ILogger<CategoriaController> logger)
        {
            _categoriaFlujo = categoriaFlujo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _categoriaFlujo.Obtener();
            if (!resultado.Any())
            {
                return NoContent();
            }
            return Ok(resultado);
        }
    }
}
