using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Column("IdCategoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("IdCategoriaPadre")]
        public int? IdCategoriaPadre { get; set; }

        [ForeignKey("IdCategoriaPadre")] // Especifica la clave foránea
        public Categoria CategoriaPadre { get; set; }

        public ICollection<Categoria> CategoriasHijas { get; set; }
    }
}
