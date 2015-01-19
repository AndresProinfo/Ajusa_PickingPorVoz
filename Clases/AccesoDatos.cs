using System;
using System.Collections.Generic;
using System.Linq;
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
        public DataTable dt;

        public AccesoDatos()
        {
            //constructor datos
            cadena="Data Source=(local);Initial Catalog=bender;Integrated Security=True";
        }

        private void conectar()
        {
            cn = new SqlConnection(cadena);
        }

        //ejecutar un procedimiento almacenado
        public void Consultar_x_codigo(string valor, string tabla)
        {
            string strSQL;
            strSQL="sp_consultar_persona";
            conectar();
            da = new SqlDataAdapter(strSQL, cn);
            cn.Open();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@codigo",SqlDbType.Int).Value=valor;
            da.Fill(ds,tabla);
        }

        public void CargaDatos()
        {

        }


        //metodo para consultar
        public void consultar(string sql, string tabla)
        {
            conectar();
            ds.Tables.Clear();
            da = new SqlDataAdapter(sql, cn);
            cmb = new SqlCommandBuilder(da);
            da.Fill(ds, tabla);
        }

        //consulta 2
        public DataTable consultar2(string tabla)
        {
            conectar();
            string sql = "select * from " + tabla;
            da = new SqlDataAdapter(sql, cn);
            DataSet dts = new DataSet();
            da.Fill(dts, tabla);
            DataTable dt = new DataTable();
            dt = dts.Tables[tabla];
            return dt;
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
        //al insertar hay un trigger en el SQL que lo que hace es guardar cierta información en la tabla persons_con
        public bool insertar(string sql)
        {

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
