using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace ServicioPPV
{
    class AccesoDatos
    {
        private string cadena;
        public SqlConnection cn;
        public DataSet ds = new DataSet();
        public SqlDataAdapter da;
        public SqlCommand comando;
        

        public AccesoDatos()
        {
            //constructor datos
            cadena = Properties.Settings.Default.ConexionSQL;
        }

        public void conectar()
        {
            cn = new SqlConnection(cadena);
        }

        //ejecutar un procedimiento almacenado
        public void EjecutaPA(Tramas T, String CadenaTrama)
        {
            //https://desdeceronetsql2.wordpress.com/2012/10/25/recuperar-1-valor-devuelto-de-procedimiento-almacenado-en-c/

            string estado, descrip;
            int i;
            char[] DelimitadorCadena = { ';' };
            string[] Campos = CadenaTrama.Split(DelimitadorCadena);

            conectar();

            SqlCommand cmd = new SqlCommand(T.QueProcAlmacenado(), cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();

            //hay que recorrer el array sacando las variables y los valores
            i = 0;
            while (i < T.NumParametrosTrama())
            {
                cmd.Parameters.Add("@" + T.ParametroNombreCampo(i), SqlDbType.NVarChar).Value = Campos[i];
            }

            SqlParameter Dev_Estado = new SqlParameter("@estado", 0);
            SqlParameter Dev_Error = new SqlParameter("@error", 0);
            Dev_Estado.Direction = ParameterDirection.Output;
            Dev_Error.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(Dev_Estado);
            cmd.Parameters.Add(Dev_Error);

            cmd.ExecuteNonQuery();

            estado = cmd.Parameters("@estado").Value.ToString();
            descrip = cmd.Parameters("@error").Value.ToString();

            cn.Close();
            

        }

        //obtiene los parametros y tipos de la trama de la tabla PPV_Campos_Trama
        public DataTable RecuperaParametrosPA(int IDTipoTrama)
        {
            DataTable dt = new DataTable();
            conectar();
            string sql = "select NombreCampo,TipoCampo,OrdenEnTrama from PPV_Campos_Trama where idtipotrama =" + IDTipoTrama + " order by OrdenEnTrama";

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public int ConsultaIDTipoTrama(String ValorTipoTrama)
        {
            DataTable dt = new DataTable();
            conectar();
            string sql = "select IDTipoTrama from PPV_Tipos_Trama where valor ='" + ValorTipoTrama + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            DataRow R = dt.Rows[0];
            return Convert.ToInt32(R["IDTipoTrama"]);
        }

        public String ConsultaProcAlmacenado(String ValorTipoTrama)
        {
            DataTable dt = new DataTable();
            conectar();
            string sql = "select ProcAlmacenado from PPV_Tipos_Trama where valor ='" + ValorTipoTrama + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            DataRow R = dt.Rows[0];
            return Convert.ToString(R["ProcAlmacenado"]);
        }

        

        //metodo eliminar
        public bool eliminar(string tabla, string condicion)
        {
            conectar();
            cn.Open();
            string sql = "delete from " + tabla + " where " + condicion;
            comando = new SqlCommand(sql, cn);
            int i = comando.ExecuteNonQuery();
            cn.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //actualizar
        public bool actualizar(string tabla, string campos, string condicion)
        {
            conectar();
            cn.Open();
            string sql = "update " + tabla + " set " + campos + " where " + condicion;
            comando = new SqlCommand(sql, cn);
            int i = comando.ExecuteNonQuery();
            cn.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //insertar
        public bool InsertarTrama(string trama)
        {
            string sql = "insert into PPV_tramas_entrada values('" + trama + "',0)";
            conectar();
            cn.Open();
            
            comando = new SqlCommand(sql, cn);
            int i = comando.ExecuteNonQuery();
            cn.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
  
  
    }
}
