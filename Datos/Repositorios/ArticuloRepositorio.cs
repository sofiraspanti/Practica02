using Microsoft.Data.SqlClient;
using Practica02.Datos.Interfaces;
using Practica02.Models;
using Practica02.Utils;
using System.Data;

namespace Practica02.Datos.Repositorios
{
    public class ArticuloRepositorio : IArticulo
    {
        public bool Add(Articulo articulo)
        {
            if (articulo == null)
            {
                throw new ArgumentNullException(nameof(articulo), "El artículo no puede ser nulo.");
            }


            bool result = true;
            SqlTransaction? t = null;
            SqlConnection? cnn = null;

            try
            {
                // obtengo la conexion y la abro
                cnn = DataHelper.GetInstance().GetConnection();
                cnn.Open();

                t = cnn.BeginTransaction(); // inicia una nueva transaccion

                // configura el comando para ejecutar el procedimiento almacenado
                var cmd = new SqlCommand("SP_AGREGAR_ARTICULO", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;

                //agrego los parametros al comando
                cmd.Parameters.AddWithValue("nombre", articulo.nombre ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("precio", articulo.precioUnitario);

                // ejecuto el comando
                cmd.ExecuteNonQuery();

                t.Commit(); // confirmo la transaccion

            }
            catch (SqlException)
            {
                if (t != null) { t.Rollback(); }
                result = false;
            }
            finally
            {
                // me aseguro de cerrar la conexion si está abierta
                if (cnn != null && cnn.State == ConnectionState.Closed) { cnn.Close(); }
            }
            return result;

        }

        public bool Delete(int id)
        {
            bool result = true;
            var articulo = GetById(id);

            if (articulo == null)
            {
                Console.WriteLine("No existe el articulo");
            }

            SqlTransaction? t = null;
            SqlConnection? cnn = null;

            try
            {
                cnn = DataHelper.GetInstance().GetConnection();
                cnn.Open();

                t = cnn.BeginTransaction();

                var cmd = new SqlCommand("SP_ELIMINAR_ARTICULO", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    result = false;
                }

                t.Commit();
            }
            catch (SqlException)
            {

                if (t != null)
                {
                    t.Rollback();
                    result = false;
                }
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open) { cnn.Close(); }
            }
            return result;

        }

        public bool Update(int id, string nuevoNombre, decimal nuevoPrecio)
        {
            bool result = true;
            var articulo = GetById(id); // traigo el objeto segun el ID. GetById es un metodo que cree mas abajo

            if (articulo == null)
            {
                Console.WriteLine("El articulo seleccionado no existe.");
                result = false;
            }

            SqlTransaction? t = null;
            SqlConnection? cnn = null;

            try
            {
                // obtengo la conexion y la abro
                cnn = DataHelper.GetInstance().GetConnection();
                cnn.Open();

                // inicio la transaccion
                t = cnn.BeginTransaction();

                // configuro el comando 
                var cmd = new SqlCommand("SP_ACTUALIZAR_ARTICULO", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("nombre", nuevoNombre);
                cmd.Parameters.AddWithValue("precio", nuevoPrecio);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    result = false; // no se actualizo ningun registro
                }

                t.Commit(); // confirmo la transaccion

            }
            catch (SqlException ex)
            {

                if (t != null)
                {
                    t.Rollback();
                }
                result = false;

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Closed)
                {
                    cnn.Close();
                }

            }
            return result;
        }

        public List<Articulo> GetAll()
        {
            DataTable dt = DataHelper.GetInstance().ExecuteSPQuery("SP_CONSULTAR_ARTICULOS", null);

            var articulos = new List<Articulo>(); // la palabra var determina que le complilador le asigne el tipo de variable segun lo que hay a la derecha del =

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    var articulo = new Articulo()
                    {
                        id_art = (int)row["id_art"],
                        nombre = row["nombre"].ToString(),
                        precioUnitario = (decimal)row["precio"], // la variable a la izq del = hace referencia a la property, a la derecha, al campo de la bbdd
                    };
                    articulos.Add(articulo);
                }

            }
            return articulos;
        }

        public Articulo GetById(int id) // este metodo nos trae un articulo segun el ID
        {
            Articulo articulo = null;
            SqlConnection? cnn = null;

            try
            {
                cnn = DataHelper.GetInstance().GetConnection();
                cnn.Open();

                var cmd = new SqlCommand("SP_OBTENER_ARTICULO_ID", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        articulo = new Articulo
                        {
                            id_art = (int)reader["id_art"], // el nombre que se encuentra entre corchetes debe coindicir con el nombre de la variable en la BD!!!!
                            nombre = reader["nombre"].ToString(),
                            precioUnitario = (decimal)reader["precio"]

                        };
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();                
                    
                }
            }
            return articulo;
        }
    }
}