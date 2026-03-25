using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;

namespace Reglas
{
    public class ProductoReglas: IProductoReglas
    {
        private readonly ITipoCambioServicio _tipoCambioServicio;

        public ProductoReglas(ITipoCambioServicio tipoCambioServicio)
        {
            _tipoCambioServicio = tipoCambioServicio;
        }

        public async Task<decimal> CalcularPrecioUSD(decimal precioColones)
        {
            var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambioVenta();
            return precioColones / tipoCambio;
        }
    }
}
