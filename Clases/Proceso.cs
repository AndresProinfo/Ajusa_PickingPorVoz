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

        public string ValorPosicionTrama(String TXT, String Delimitador, int Posicion)
        {
            char[] DelimitadorCadena = { Char.Parse(Delimitador) };
            string[] Campos = TXT.Split(DelimitadorCadena);

            return Campos[Posicion].ToString();
        }
        public void Ejecutar()
        {
            string mensaje;
                try
			    {
                    _padre.RegistroActividad(FMain.Msg_Info, "Datos recibidos: " + _trama.Replace('\0', ' ') , "Proceso");
				    	            
                    //insertar la trama en la tabla "PPV_tramas_entrada
                    AccesoDatos AD = new AccesoDatos();
                    AD.InsertarTrama(_trama);
                    Tramas Tr = new Tramas(_trama.Replace('\0', ' '));
                    log.Info("Ejecución del proc. almacenado: " + Tr.QueProcAlmacenado());
                    //Tr.CargaParametrosTrama();
                    //hay que recuperar los parametros y tipos de la trama y ejecutar el PA
                    mensaje = "Datos enviados: " + AD.EjecutaPA(Tr,_trama.Replace('\0',' '));
                    
                    //envio de la trama a la pistola
                    _sck.Send(Encoding.UTF8.GetBytes(mensaje));

                    if (ValorPosicionTrama(mensaje,";",0)=="OK")
                    {
                        if (AD.CambiaStatusIDTramaEnvio(mensaje) == 1)
                        {
                            log.Info("Trama enviada correctamente a la pistola " + mensaje);
                        }
                        else
                        {
                            log.Info("La trama de confirmación no ha podido actualizarse a Status=3 en PPV_Tramas_Envio "+mensaje);
                        }
                   
                    }
                    _padre.RegistroActividad(FMain.Msg_Info, mensaje);
            
                    _sck.Close();
			    }
			    catch (Exception E)
			    {
                    log.Error("Error al enviar datos. " + E.Message);
				    _padre.RegistroActividad(FMain.Msg_Error, "Error al enviar datos. " + E.Message, "Proceso");
			    }
        }

	}
}

	