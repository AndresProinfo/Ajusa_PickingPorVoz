using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Utils;

namespace ServicioPPV
{
	class Proceso
	{
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket _sck;
        private String _trama;
        private FMain _padre;



		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sck">Socket al que enviar la respuesta</param>
		/// <param name="trama">Petición</param>
		/// <param name="padre">Formulari opadre</param>
		public Proceso(Socket sck, String trama, FMain padre)
		{
              _sck = sck;
              _trama = trama;
              _padre = padre;
		}

        public void Ejecutar()
        { 
			    
                try
			    {
                    _padre.RegistroActividad(FMain.Msg_Info, "Datos recibidos: " + _trama.Replace('\0', ' ') , "Proceso");
				    //Thread.Sleep(5000);
	            
                    //insertar la trama en la tabla "PPV_tramas_entrada
                    AccesoDatos AD = new AccesoDatos();
                    AD.InsertarTrama(_trama);
                    Tramas Tr = new Tramas(_trama.Replace('\0', ' '));
                    log.Info("Ejecución del proc. almacenado: " + Tr.QueProcAlmacenado());
                    Tr.CargaParametrosTrama();
                    //hay que recuperar los parametros y tipos de la trama y ejecutar el PA
                    Tr.EjecutaPA();




                    _padre.RegistroActividad(FMain.Msg_Info, "Ejecución del Proc. Alm: " + Tr.QueProcAlmacenado());
                    
                    //_sck.Send(Encoding.Default.GetBytes(Tr.QueProcAlmacenado()));
                    _padre.RegistroActividad(FMain.Msg_Info, "Enviado trama de confirmación. ");
                    _sck.Close();
			    }
			    catch (Exception E)
			    {
				    _padre.RegistroActividad(FMain.Msg_Error, "Error al enviar datos. " + E.Message, "Proceso");
			    }
        }

	}
}

	