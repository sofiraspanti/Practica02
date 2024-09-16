using Practica02.Datos.Interfaces;
using Practica02.Datos.Repositorios;
using Practica02.Models;

namespace Practica02.Servicios
{
    public class ArticuloServicio
    {
        private readonly IArticulo _articuloRepositorio;

        public ArticuloServicio(IArticulo articuloRepositorio)
        {

            _articuloRepositorio = articuloRepositorio ?? throw new ArgumentNullException(nameof(articuloRepositorio));

        }

        public List<Articulo> GetAllArticulo()
        {
            return _articuloRepositorio.GetAll();
        }

        public bool AddArticulo(Articulo articulo)
        {
            return _articuloRepositorio.Add(articulo);
        }

        public bool UpdateArticulo (int id, string nombre, decimal precio)
        {
            return _articuloRepositorio.Update(id, nombre, precio);
        }

        public bool DeteleArticulo(int id)
        {
            return _articuloRepositorio.Delete(id);
        }

        public Articulo GetById(int id)
        {
            return _articuloRepositorio.GetById(id);
        }
    }
}
