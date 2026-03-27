using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Web.Pages.Productos
{
     [Authorize]
    public class EditarModel : PageModel
    {
        private IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoResponse producto { get; set; } = default!;

        public ProductoRequest productoRequest { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaseleccionada { get; set; }

        [BindProperty]
        public Guid subCategoriaseleccionada { get; set; }

        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "ObtenerProducto");

            var cliente = ObtenerClienteConToken();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                await ObtenerCategoriasAsync();

                var resultado = await respuesta.Content.ReadAsStringAsync();

                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                producto = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones);

                if (producto != null)
                {
                    var categoria = categorias.FirstOrDefault(m => m.Text == producto.Categoria);

                    if (categoria != null)
                    {
                        categoriaseleccionada = Guid.Parse(categoria.Value);

                        var resultadoSub = await ObtenersubCategoriasAsync(categoriaseleccionada);

                        subCategorias = resultadoSub.Select(m => new SelectListItem
                        {
                            Value = m.Id.ToString(),
                            Text = m.Nombre,
                            Selected = m.Nombre == producto.SubCategoria
                        }).ToList();

                        var subCategoria = subCategorias.FirstOrDefault(m => m.Text == producto.SubCategoria);

                        if (subCategoria != null)
                        {
                            subCategoriaseleccionada = Guid.Parse(subCategoria.Value);
                        }
                    }
                }
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (producto.Id == Guid.Empty)
                return NotFound();


            if (!ModelState.IsValid)
            {
                await ObtenerCategoriasAsync();

                if (categoriaseleccionada != Guid.Empty)
                {
                    var resultado = await ObtenersubCategoriasAsync(categoriaseleccionada);
                    subCategorias = resultado.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre,
                        Selected = a.Id == subCategoriaseleccionada
                    }).ToList();
                }

                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "EditarProducto");

            var cliente = ObtenerClienteConToken();

            var respuesta = await cliente.PutAsJsonAsync(
                string.Format(endpoint, producto.Id.ToString()),
                new ProductoRequest
                {
                    IdSubCategoria = subCategoriaseleccionada,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CodigoBarras = producto.CodigoBarras       

                });

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        private async Task ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "ObtenerCategorias");
            var cliente = ObtenerClienteConToken();
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
                        Text = m.Nombre
                    }).ToList();
            }
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaId)
        {
            var subCategorias = await ObtenersubCategoriasAsync(categoriaId);
            return new JsonResult(subCategorias);
        }

        private async Task<List<SubCategoria>> ObtenersubCategoriasAsync(Guid categoriaId)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoint", "ObtenerSubCategorias");
            var cliente = ObtenerClienteConToken();
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

        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Token");
            var cliente = new HttpClient();
            if (tokenClaim != null)
                cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", tokenClaim.Value);
            return cliente;
        }
    }
}