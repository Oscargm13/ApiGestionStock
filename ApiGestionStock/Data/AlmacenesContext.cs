using ApiGestionStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionStock.Data
{
    public class AlmacenesContext: DbContext
    {
        public AlmacenesContext(DbContextOptions<AlmacenesContext> options): base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
