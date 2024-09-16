using Practica02.Models;

namespace Practica02.Datos.Interfaces
{
    public interface IArticulo
    {
        bool Add(Articulo articulo); // Agregar articulo
        List<Articulo> GetAll(); // Consultar todos los articulos

        bool Update (int id, string nuevoNombre, decimal nuevoPrecio); // Editar articulo

        bool Delete (int id); // Dar de baja un producto

        Articulo GetById (int id);
    

    }
}
