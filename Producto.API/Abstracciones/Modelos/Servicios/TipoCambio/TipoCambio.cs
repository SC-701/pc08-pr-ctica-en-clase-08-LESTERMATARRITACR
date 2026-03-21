namespace Abstracciones.Modelos.Servicios.TipoCambio
{
    public class TipoCambioResponse
    {
        public List<Dato> Datos { get; set; }
    }

    public class Dato
    {
        public List<Indicador> Indicadores { get; set; }
    }

    public class Indicador
    {
        public List<Serie> Series { get; set; }
    }

    public class Serie
    {
        public decimal ValorDatoPorPeriodo { get; set; }
        public string Fecha { get; set; }
    }
}
