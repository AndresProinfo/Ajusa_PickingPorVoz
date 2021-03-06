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

namespace VoiceDispatcher
{
	class Proceso
	{
    private Socket _sck;
    private String _trama;
    private FMain _padre;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sck">Socket al que enviar la respuesta</param>
		/// <param name="trama">Petici�n</param>
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
        _padre.RegistroActividad(FMain.Msg_Info, "Datos recibidos: " + _trama.Replace('\0', ' ') + " .Esperando 5 segundos para contestar", "Proceso");
				Thread.Sleep(5000);
		    _sck.Send(Encoding.Default.GetBytes(_trama));
        _sck.Close();
			}
			catch (Exception E)
			{
				_padre.RegistroActividad(FMain.Msg_Error, "Error al enviar datos. " + E.Message, "Proceso");
			}
    }

	}
}

	