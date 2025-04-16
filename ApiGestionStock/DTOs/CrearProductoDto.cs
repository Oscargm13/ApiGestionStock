namespace ApiGestionStock.DTOs
{
    public class CrearProductoDto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal Coste { get; set; }
        public string Imagen { get; set; }

        // Asociación directa
        public int? IdCategoria { get; set; }

        // Para crear nueva categoría
        public string? NombreCategoria { get; set; }
        public int? IdCategoriaPadre { get; set; }

        // Proveedor asociado
        public int? IdProveedor { get; set; }

        // Tiendas asociadas
        public List<int>? IdsTiendas { get; set; }
    }
}
