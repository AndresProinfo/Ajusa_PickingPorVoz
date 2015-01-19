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
        private SqlCommandBuilder cmb;
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
        public void EjecutaPA(Tramas T)
        {
            int i;
            conectar();
            
            da = new SqlDataAdapter(T.QueProcAlmacenado(), cn);
            cn.Open();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            
            //hay que recorrer el array sacando las variables y los valores
            i = 0;
            while (i < T.NumParametrosTrama())
            {
                da.SelectCommand.Parameters.Add("@"+t.




            }
            da.SelectCommand.Parameters.Add("@codigo",SqlDbType.Int).Value=1;
            da.Fill(ds,"tabla");
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
