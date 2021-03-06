﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;



namespace ServicioPPV
{
    class Tramas
    {
        #region VbleLocal
            private string TipoTrama;
            private string ProcAlmacenado;
            private int IDTipoTrama;
            private int NumParametros;
            string[,] Campos = new string[Properties.Settings.Default.CantMaxParametros,3];
        
        #endregion

        #region Metodos

            public int NumParametrosTrama()
            {
                return NumParametros;
            }
            public Tramas(string trama)
            {
                char[] DelimitadorCadena = { ';' };
                string[] Campos = trama.Split(DelimitadorCadena);
                AccesoDatos AD = new AccesoDatos();
                
                TipoTrama = Campos[2];
                ProcAlmacenado = AD.ConsultaProcAlmacenado(TipoTrama);
                IDTipoTrama = AD.ConsultaIDTipoTrama(TipoTrama);

                XCargaParametrosTrama();
              
            }

            public string QueProcAlmacenado()
            {
               return ProcAlmacenado;
            }

            public void XCargaParametrosTrama()
            {
                AccesoDatos AD = new AccesoDatos();
                
                DataTable DT = new DataTable();
                DT = AD.RecuperaParametrosPA(IDTipoTrama);
                NumParametros = DT.Rows.Count;  //numero de parametros de la Trama
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    //- Guardar la Columna Nombre el el Arreglo
                    Campos[i,0] = DT.Rows[i]["NombreCampo"].ToString();
                    Campos[i,1] = DT.Rows[i]["TipoCampo"].ToString();
                    Campos[i,2] = DT.Rows[i]["OrdenEnTrama"].ToString();
                }
            }
            
            public string ParametroNombreCampo(int i)
            {
                return Campos[i, 0].ToString();
            }
            public string ParametroTipoCampo(int i)
            {
                return Campos[i, 1].ToString();
            }
            public string ParametroOrdenEnTrama(int i)
            {
                return Campos[i, 2].ToString();
            }
        #endregion
    }
}
