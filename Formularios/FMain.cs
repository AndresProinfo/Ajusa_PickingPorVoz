using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;
using Utils;




[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ServicioPPV
{
	public partial class FMain : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region VblesPrivadas
        private static string IPSCK;
        private static string PuertoSCK;
           
        #endregion 

        #region Constructor

        /// <summary>
		/// Constructor
		/// </summary>
		public FMain()
		{
			InitializeComponent();

            IPSCK = Properties.Settings.Default.IPSCK;
            PuertoSCK = Properties.Settings.Default.PuertoSCK;
            try
            {
                SCK = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketCallbackReceive = new AsyncCallback(OnDataReceived);
                //accion despues de recibir
                SocketCallbackEnd = new AsyncCallback(OnDisconnect);

                this.Width = 670;
                this.Height = 420;

                LimpiarLog(5);
            }
            catch (Exception E)
            {
                MessageBox.Show(
                    "Error al iniciar la aplicación.\r\n\r\n" + E.Message,
                    "Iniciar aplicación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
		}

		#endregion

		#region Constantes

		// Constantes estáticas
		private const int ST_Iniciando	= 0;
		private const int ST_Iniciado		= 1;
		private const int ST_Deteniendo = 2;
		private const int ST_Detenido		= 3;
		private const int ST_Salir			= 4;

		public const int Msg_Info				= 0;
		public const int Msg_Error			= 1;
		public const int Msg_Warning		= 2;
		public const int Msg_Command		= 3;

		static readonly String[] Msg		= new String[4] { "INF", "ERR", "WRN", "CMD" };

		public const int MAX_DATA				= 1024;

		#endregion

		#region Variables públicas
		
		// Parámetros SGAServer
		public String			SGAServidorTCP;
		public int				SGAPuertoTCP;
		public int				SGATimeOut;

		// Parámetros PLCServer
		public String			PLCServidorTCP;
		public int				PLCPuertoTCP;
		public int				PLCTimeOut;
		
		// POP
		public int				Congestion;
		public bool				IntegrarPLC;

    //Parámetros lectores
    public String     NSInicio;
    public String     NSFin;
    public String     NSSTX;
    public String     NSETX;
    public int        NSTimeOut;

		#endregion

		#region Variables privadas

		private int		 		Estado;

		// Socket  
		private Socket				SCK;
		private AsyncCallback SocketCallbackReceive;
		private AsyncCallback SocketCallbackEnd;

		#endregion

		#region Eventos

		private void FMain_Shown(object sender, EventArgs e)
		{
			
		}

		private void FMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (MessageBox.Show("¿Desea salir de la aplicación?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
        CerrarSockets();
			}
      else
        e.Cancel = true;
		}

		private void FMain_Resize(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
				this.Hide();
		}

		private void tIcon_DoubleClick(object sender, EventArgs e)
		{
			this.Show();
			this.WindowState = FormWindowState.Normal;
		}

        private void button1_Click(object sender, EventArgs e)
        {
            Iniciar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistroActividad(Msg_Info, "Cerrando el servicio ...");
            CerrarSockets();
            Estado = ST_Detenido;
            RegistroActividad(Msg_Info, "Servicio Cerrado ...");
        }

		#endregion

		#region Métodos

		private void Iniciar()
		{
			try
			{
                log.Info("Arrancando el servicio de SCK");
                RegistroActividad(Msg_Info, "Iniciando servicio ...");
				Estado = ST_Iniciando;

				CerrarSockets();

				SCK = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SCK.Bind(new IPEndPoint(IPAddress.Parse(IPSCK), Convert.ToInt32(PuertoSCK)));
				SCK.Listen(4);
				SCK.BeginAccept(new AsyncCallback(OnClientConnect), null);

				Estado = ST_Iniciado;
				RegistroActividad(Msg_Info, "========== Servicio iniciado ==========");
                RegistroActividad(Msg_Info, "Servidor socket iniciado en la IP: " + IPSCK + " puerto: "+ PuertoSCK);
			}
			catch (Exception E)
			{
				MessageBox.Show(
						"Error al iniciar el servicio Picking Por Voz.\r\n\r\n" + E.Message,
						"Iniciando servicio",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
			}
		}

		#endregion

		#region Log

		/// <summary>
		/// Insertamos un mensaje en el log de enviados
		/// Insertamos un mensaje en el fichero de log
		/// </summary>
		/// <param name="Tipo">Tipo de mensaje</param>
		/// <param name="Texto">Texto del mensaje</param>
		public void RegistroActividad(int Tipo, String Texto)
		
		{
			DateTime			dt;
			ListViewItem	id;
			StreamWriter	swWriter;

			dt = DateTime.Now;

			id = new ListViewItem();
			id.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
			id.SubItems.Add(Texto);
			id.ImageIndex = Tipo;

			SetIdREG(id);

			try
			{
				swWriter = File.AppendText(Rutinas.AppPath() + "LOGS//" + Rutinas.AppName() + "_" + dt.ToString("yyyyMMdd") + ".log");
				swWriter.WriteLine(Msg[Tipo] + " " + dt.ToString("HH:mm:ss") + " " + Texto);
				swWriter.Close();
			}
			catch (Exception E)
			{
				id = new ListViewItem();
				id.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
				id.SubItems.Add("Error al escribir el fichero de log. " + E.Message);
				id.ImageIndex = Msg_Error;
				SetIdREG(id);
			}
		}

		/// <summary>
		/// Insertamos un mensaje en el log de enviados
		/// Insertamos un mensaje en el fichero de log
		/// </summary>
		/// <param name="Tipo">Tipo de mensaje</param>
		/// <param name="Texto">Texto del mensaje</param>
		/// <param name="SERNUNMB">SN terminal</param>
		public void RegistroActividad(int Tipo, String Texto, String SERNUNMB)
		
		{
			DateTime			dt;
			ListViewItem	id;
			StreamWriter	swWriter;

			dt = DateTime.Now;

			id = new ListViewItem();
			id.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
			id.SubItems.Add(Texto);
			id.ImageIndex = Tipo;

			SetIdREG(id);

			try
			{
				swWriter = File.AppendText(Rutinas.AppPath() + "LOGS//" + SERNUNMB + "_" + dt.ToString("yyyyMMdd") + ".log");
				swWriter.WriteLine(Msg[Tipo] + " " + dt.ToString("HH:mm:ss") + " " + Texto);
				swWriter.Close();
			}
			catch (Exception E)
			{
				id = new ListViewItem();
				id.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
				id.SubItems.Add("Error al escribir el fichero de log. " + E.Message);
				id.ImageIndex = Msg_Error;
				SetIdREG(id);
			}
		}

		/// <summary>
		/// Método para limpiar logs antiguos
		/// </summary>
		/// <param name="dias">Dias de antiguedad a partir del cual debemos eliminar el log</param>
		private void LimpiarLog(int dias)
		{
			DateTime	dt;
			String[]	ficheros = null;
			FileInfo	fi = null;
			
			try
			{ 
				dt = DateTime.Now;
				ficheros = Directory.GetFiles(Rutinas.AppPath()+ "LOGS//", "*.log");

				for (int i = 0; i < ficheros.Length; i++)
				{
					fi = new FileInfo(ficheros[i]);
					if (fi.LastWriteTime.AddDays(dias) < dt)
						fi.Delete();
				}
			}
			catch 
			{
			}
		}

		#endregion

		#region Socket

		public class SocketPacket
		{
			public Socket Soc;
			public byte[] Buffer = new byte[MAX_DATA];
		}

		private void OnClientConnect(IAsyncResult asyn)
		{
			try
			{
				if (Estado != ST_Deteniendo && Estado != ST_Detenido)
				{
                    RegistroActividad(Msg_Info, "Nueva conexión");
					WaitForData(SCK.EndAccept(asyn));
					SCK.BeginAccept(new AsyncCallback(OnClientConnect), null);
				}
			}
			catch (Exception E)
			{
				RegistroActividad(Msg_Error, "Error al iniciar la nueva conexión socket." + E.Message);

				try
				{
					SCK.BeginAccept(new AsyncCallback(OnClientConnect), null);
				}
				catch (Exception E2)
				{ 
					RegistroActividad(Msg_Error, "Error al reiniciar la nueva conexión socket." + E2.Message);
				}
			}
		}

		private void WaitForData(Socket soc)
		{
			try
			{
				if (Estado != ST_Deteniendo && Estado != ST_Detenido)
				{
					SocketPacket sp = new SocketPacket();
					sp.Soc = soc;
					soc.BeginReceive(sp.Buffer, 0, sp.Buffer.Length, SocketFlags.None, SocketCallbackReceive, sp);
				}
			}
			catch (Exception E)
			{
				RegistroActividad(Msg_Error, "Error al procesar la nueva conexión socket." + E.Message);
				//SCK.BeginDisconnect(false, new AsyncCallback(OnDisconnect), soc);
			}
		}

		private void OnDataReceived(IAsyncResult asyn)
		{
			SocketPacket	sp = null;
			int						iRX = 0;
			int						charLen = 0;
			char[]				chars;
			Decoder				d;
			String				cmd = "";

			try
			{
				if (Estado != ST_Deteniendo && Estado != ST_Detenido)
				{
					sp = (SocketPacket)asyn.AsyncState;

					iRX = sp.Soc.EndReceive(asyn);
					chars = new char[iRX + 1];
					d = Encoding.Default.GetDecoder();
					charLen = d.GetChars(sp.Buffer, 0, iRX, chars, 0);
					cmd = new String(chars);

					// Proceso
					Proceso p = new Proceso(sp.Soc, cmd, this);
                    Thread t = new Thread(new ThreadStart(p.Ejecutar));
                    t.Start();

				}
			}
			catch (Exception E)
			{
				RegistroActividad(Msg_Error, "Error al leer datos de la conexión socket." + E.Message);
			}
		}		

		private void OnDisconnect(IAsyncResult asyn)
		{
			Socket soc;
            try
            {
                if (Estado != ST_Detenido && Estado != ST_Deteniendo)
                {
                    soc = (Socket)asyn.AsyncState;
                    soc.EndDisconnect(asyn);
                }
            }
            catch (Exception E)
            {
                RegistroActividad(Msg_Error, "Error al enviar datos de respuesta." + E.Message);
            }
		}

		private void CerrarSockets()
		{
            log.Info("Cerrando el servicio de SCK");
            if (SCK != null)
				SCK.Close();
		}

		#endregion

		#region Delegate

		// Delegates
		delegate void SetIdREGCallback(ListViewItem id);

		private void SetIdREG(ListViewItem id)
		{
			if (lREG.InvokeRequired)
			{
				SetIdREGCallback d = new SetIdREGCallback(SetIdREG);
				this.Invoke(d, new object[] { id });
			}
			else
			{
				if (lREG.Items.Count > 100)
					lREG.Items.RemoveAt(0);
				lREG.Items.Add(id);

				id.EnsureVisible();
			}
		}

		#endregion

        private void FMain_Load(object sender, EventArgs e)
        {

        }

     

    }
}