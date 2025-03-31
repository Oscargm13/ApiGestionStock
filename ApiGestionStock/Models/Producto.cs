using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Column("Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El coste es obligatorio")]
        [Column("Coste")]
        public decimal Coste { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [StringLength(100, ErrorMessage = "La ruta de la imagen no puede tener más de 100 caracteres")]
        [Column("Imagen")]
        public string Imagen { get; set; }


    }
}
