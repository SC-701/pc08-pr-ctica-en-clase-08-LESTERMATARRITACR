using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "La propiedad nombre es requerida")]
        [StringLength(60, ErrorMessage = "La propiedad nombre debe tener entre 3 y 60 caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La propiedad descripcion es requerida")]
        [StringLength(200, ErrorMessage = "La propiedad descripcion debe tener entre 5 y 200 caracteres", MinimumLength = 5)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La propiedad precio es requerida")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "La propiedad stock es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "La propiedad stock debe ser un número entero no negativo")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "La propiedad codigobarras es requerida")]
        [RegularExpression(@"^[0-9]{5,13}$", ErrorMessage = "La propiedad codigobarras debe tener entre 5 y 13 dígitos")]
        public string CodigoBarras { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        public Guid Id { get; set; }
        public string SubCategoria { get; set; }
        public string Categoria { get; set; }
    }

    public class ProductoDetalle : ProductoResponse
    {
        public decimal PrecioUSD { get; set; }
      
    }
}
    
   
