using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestionStock.Models
{
    [Table("Notificaciones")]
    public class Notificacion
    {
        [Key]
        [Column("IdNotificacion")]
        public int IdNotificacion { get; set; }

        [Required]
        [Column("Mensaje")]
        public string Mensaje { get; set; }

        [Required]
        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("IdProducto")]
        public int? IdProducto { get; set; }

        [Column("IdTienda")]
        public int? IdTienda { get; set; }
    }
}
