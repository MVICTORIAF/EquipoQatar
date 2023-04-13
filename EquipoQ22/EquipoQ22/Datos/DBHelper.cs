using EquipoQ22.Domino;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Datos
{
    class DBHelper
    {
        private SqlConnection conn;
        private string CadenaConexion = @"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=Qatar2022;Integrated Security=True";
        private static DBHelper instancia;

        public DBHelper()
        {
            conn = new SqlConnection(CadenaConexion);
        }

        public static DBHelper ObtenerInstancia()
        {
            if (instancia == null)

                instancia = new DBHelper();
            return instancia;

        }

        public bool Confirmar(Equipo oEquipo)
        {
            bool resultado = true;

            SqlConnection cnn = new SqlConnection();
            SqlTransaction trans = null;

            try
            {
                cnn.ConnectionString = @"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=Qatar2022;Integrated Security=True";
                cnn.Open();
                trans = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();

                cmd.Transaction = trans;
                cmd.Connection = cnn;
                cmd.CommandText = "SP_INSERTAR_EQUIPO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pais", oEquipo.pais);
                cmd.Parameters.AddWithValue("@director_tecnico", oEquipo.director);

                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@id";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOut);
                cmd.ExecuteNonQuery();

                int nroJugador = (int)pOut.Value;

                SqlCommand cmdDetalle;
                int nnroJugadorro = 1;

                foreach (Jugador item in oEquipo.Detalles)
                {
                    SqlCommand cmdDet = new SqlCommand();
                    cmdDet.Connection = cnn;
                    cmdDet.Transaction = trans;
                    cmdDet.CommandText = "SP_INSERTAR_DETALLES_EQUIPO";
                    cmdDet.CommandType = CommandType.StoredProcedure;
                    cmdDet.Parameters.AddWithValue("@id_equipo", nroJugador);
                    cmdDet.Parameters.AddWithValue("@id_persona", item.Persona.IdPersona);
                    cmdDet.Parameters.AddWithValue("@camiseta", item.Camiseta);
                    cmdDet.Parameters.AddWithValue("@posicion", item.Posicion);
                    cmdDet.ExecuteNonQuery();

                }

                trans.Commit();
                cnn.Close();

            }
            catch (Exception)
            {
                trans.Rollback();
                resultado = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }

            return resultado;
        }

        public DataTable ConsultarDB(string NomProc)
        {
            DataTable tabla = new DataTable();
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = NomProc;
            cmd.CommandType = CommandType.StoredProcedure;
            tabla.Load(cmd.ExecuteReader());
            conn.Close();
            return tabla;
        }
    }
}
