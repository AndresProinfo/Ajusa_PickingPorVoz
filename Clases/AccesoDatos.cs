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
        public string EjecutaPA(Tramas T, String CadenaTrama)
        {
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
                cmd.Parameters.Add("@" + T.ParametroNombreCampo(i).ToString(), SqlDbType.NVarChar, 20).Value = Campos[i].ToString();
                i++;
            }
                                  
            SqlParameter Dev_Estado = new SqlParameter("@estado",SqlDbType.NVarChar,20);
            SqlParameter Dev_Error = new SqlParameter("@error", SqlDbType.NVarChar,400);
            Dev_Estado.Direction = ParameterDirection.Output;
            Dev_Error.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(Dev_Estado);
            cmd.Parameters.Add(Dev_Error);

            cmd.ExecuteNonQuery();

            estado = Dev_Estado.Value.ToString();
            descrip = Dev_Error.Value.ToString();

            cn.Close();
            return estado + ";" + descrip; //+ ";" + CadenaTrama;

        }
        public int CambiaStatusIDTramaEnvio(string Trama)
        {

            conectar();
            SqlCommand cmd = new SqlCommand("PPV_ActualizaStatusID", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cmd.Parameters.Add("@trama", SqlDbType.NVarChar, 255).Value = Trama.ToString();
            SqlParameter Dev_Estado = new SqlParameter("@StatusOK", SqlDbType.Int, 0);
            Dev_Estado.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(Dev_Estado);

            cmd.ExecuteNonQuery();
            int valor;
            valor = Int32.Parse(Dev_Estado.Value.ToString());
           
            cn.Close();
            return valor;
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
