using Microsoft.Data.SqlClient;
using System.Data;

namespace Practica02.Utils
{
    public class DataHelper
    {
        private static DataHelper _instancia;
        private SqlConnection _connection;

        private DataHelper()
        {
            _connection = new SqlConnection("Data Source=210826-013159\\SQLEXPRESS;Initial Catalog=Practica01;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }

        public static DataHelper GetInstance()
        {
            if (_instancia == null)
                _instancia = new DataHelper();

            return _instancia;
        }

        public DataTable ExecuteSPQuery(string sp, List<ParameterSQL>? parametros)
        {
            DataTable t = new DataTable();
            SqlCommand cmd = null;

            try
            {
                _connection.Open();
                cmd = new SqlCommand(sp, _connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var param in parametros)
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                }

                t.Load(cmd.ExecuteReader());
            }
            catch (SqlException)
            {
                t = null;
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return t;
        }



        public int ExecuteSPDML(string sp, List<ParameterSQL>? parametros)
        {
            int rows;
            try
            {
                _connection.Open();
                var cmd = new SqlCommand(sp, _connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (parametros != null)
                {
                    foreach (var param in parametros)
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                }

                rows = cmd.ExecuteNonQuery();
                _connection.Close();
            }
            catch (SqlException)
            {
                rows = 0;
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return rows;
        }
        public SqlConnection GetConnection()
        {
            return _connection;
        }
    }
}
