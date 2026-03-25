using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private IConfiguracion _configuracion;
        [BindProperty]
        public ProductoRequest producto { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = default!;
        public Guid categoriaSeleccionada { get; set; } = default!;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<ActionResult> OnGet()
        {
            await ObtenerCategoriasAsync();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "AgregarProducto");
            var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(endpoint, producto);
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
        }

        private async Task ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "ObtenerCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadoDeserializado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);
                categorias = resultadoDeserializado.Select(m =>
                                  new SelectListItem
                                  {
                                      Value = m.Id.ToString(),
                                      Text = m.Nombre.ToString()
                                  }).ToList();
            }
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaId)
        {
            var subCategorias = await ObtenerSubCategorias(categoriaId);
            return new JsonResult(subCategorias);
        }

        private async Task<List<SubCategoria>> ObtenerSubCategorias(Guid categoriaId)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "ObtenerSubCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, categoriaId));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SubCategoria>>(resultado, opciones);
            }
            return new List<SubCategoria>();
        }
    }
}