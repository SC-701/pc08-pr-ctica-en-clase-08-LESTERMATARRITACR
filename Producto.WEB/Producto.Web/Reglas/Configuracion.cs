using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Servicios;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private IConfiguration _configuracion;

        public Configuracion(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        public string ObtenerMetodo(string seccion, string nombre)
        {
            var UrlBase = ObtenerValor(seccion);

            var Metodo = _configuracion
                .GetSection(seccion)
                .Get<ApiEndPoint>()
                .Metodos
                .FirstOrDefault(m => m.Nombre == nombre)
                .Valor;

            return $"{UrlBase}{Metodo}";
        }

        public string ObtenerValor(string llave)
        {
            return _configuracion.GetSection(llave).Get<ApiEndPoint>().UrlBase;
        }
    }
}