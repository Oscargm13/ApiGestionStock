using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionStock.Models
{
    [Table("ManagersTiendas")]
    [PrimaryKey(nameof(IdUsuario), nameof(IdTienda))]
    public class ManagerTienda
    {
        [ForeignKey("IdUsuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdTienda")]
        public int IdTienda { get; set; }

        public Usuario Usuario { get; set; }
        public Tienda Tienda { get; set; }
    }
}
