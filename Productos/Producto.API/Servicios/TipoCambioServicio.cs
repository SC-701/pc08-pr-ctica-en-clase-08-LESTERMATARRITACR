using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.TipoCambio;
using Reglas;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text.Json;

namespace Servicios
{
    public class TipoCambioServicio : ITipoCambioServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClient;

        public TipoCambioServicio(IHttpClientFactory httpClient, IConfiguracion configuracion)
        {
            _configuracion = configuracion;
            _httpClient = httpClient;
        }

        public async Task<decimal> ObtenerTipoCambioVenta()
        {
            var urlBase = _configuracion.ObtenerMetodo("BancoCentralCR", "ObtenerTipoCambio");

            string fecha = DateTime.Now.ToString("yyyy/MM/dd");
            string url = $"{urlBase}?fechaInicio={fecha}&fechaFin={fecha}&idioma=ES";

            var cliente = _httpClient.CreateClient();

            var token = _configuracion.ObtenerValor("BancoCentralCR:BearerToken");

            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var respuesta = await cliente.GetAsync(url);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultadoDeserializado =
                JsonSerializer.Deserialize<TipoCambioResponse>(resultado, opciones);

            return resultadoDeserializado
                .Datos.FirstOrDefault()
                .Indicadores.FirstOrDefault()
                .Series.FirstOrDefault()
                .ValorDatoPorPeriodo;
        }
    }
}