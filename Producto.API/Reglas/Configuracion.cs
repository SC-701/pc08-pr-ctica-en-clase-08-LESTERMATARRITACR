using Abstracciones.Interfaces.Reglas;
using Microsoft.Extensions.Configuration;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private IConfiguration _configuration;

        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerMetodo(string seccion, string nombre)
        {
          
            return _configuration.GetSection(seccion).GetSection("UrlBase").Value;
        }

        public string ObtenerValor(string llave)
        {
            return _configuration.GetSection(llave).Value;
        }
    }
}
