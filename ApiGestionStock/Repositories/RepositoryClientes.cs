using ApiGestionStock.Data;
using ApiGestionStock.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiGestionStock.Repositories
{
    public class RepositoryClientes
    {
        private AlmacenesContext context;
        public RepositoryClientes(AlmacenesContext context)
        {
            this.context = context;
        }
        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await this.context.Clientes.ToListAsync();
        }

        public async Task<Cliente> FindClienteAsync(int id)
        {
            return await this.context.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
        }

        public async Task CreateClienteAsync(int idCliente, string nombre, string apellido, string email, string direccion,
            string telefono, DateTime fechaNacimiento, string genero)
        {
            Cliente cliente = new Cliente();
            //cliente.IdCliente = idCliente;
            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Email = email;
            cliente.Direccion = direccion;
            cliente.Telefono = telefono;
            cliente.FechaNacimiento = fechaNacimiento;
            cliente.Genero = genero;
            await this.context.Clientes.AddAsync(cliente);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(int idCliente, string nombre, string apellido, string email, string direccion,
            string telefono, DateTime fechaNacimiento, string genero)
        {
            Cliente cliente = await this.FindClienteAsync(idCliente);
            cliente.IdCliente = idCliente;
            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Email = email;
            cliente.Direccion = direccion;
            cliente.Telefono = telefono;
            cliente.FechaNacimiento = fechaNacimiento;
            cliente.Genero = genero;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(int id)
        {
            Cliente cliente = await this.FindClienteAsync(id);
            this.context.Clientes.Remove(cliente);
            await this.context.SaveChangesAsync();
        }
    }
}
