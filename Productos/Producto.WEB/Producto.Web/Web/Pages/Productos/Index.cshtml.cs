using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
         
        }

        public IList<ProductoResponse> productos { get; set; } = default!;
        

        public async Task OnGet()
        {
            string endPoint = _configuracion.ObtenerMetodo("ApiEndPoint", "ObtenerProductos");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endPoint);
           
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                productos = JsonSerializer.Deserialize<List<ProductoResponse>>(resultado, opciones);
            }
        }
    }
}
