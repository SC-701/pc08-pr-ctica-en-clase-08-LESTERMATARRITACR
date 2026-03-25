using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriaController : ControllerBase, ISubCategoriaController
    {
        private ISubCategoriaFlujo _subCategoriaFlujo;
        private ILogger<CategoriaController> _logger;

        public SubCategoriaController(ISubCategoriaFlujo subCategoriaFlujo, ILogger<CategoriaController> logger)
        {
            _subCategoriaFlujo = subCategoriaFlujo;
            _logger = logger;
        }

        [HttpGet("{IdCategoria}")]
        public async Task<IActionResult> Obtener(Guid IdCategoria)
        {
            var resultado = await _subCategoriaFlujo.Obtener(IdCategoria);
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }
    }
}
