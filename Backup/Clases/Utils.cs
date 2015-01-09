using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace Utils
{
	#region Rutinas
	/// <summary>
	/// Clase con rutinas varias
	/// </summary>
	public class Rutinas
	{
		#region Constantes

		public const String STX = "\x02";
		public const String ETX = "\x03";

		#if DEBUG
			public const String ENQ = "\x05";
		#else
			public const String ENQ = "\x05";
		#endif

		public const String SPM = "\x08";
		public const String EPM = "\x09";
		public const String ENT = "\r\n\0";
		public const String END = "\r\n\r\n\0";
		public const String NL  = "\r\n";
    public const String CR  = "\r";

		public const char		ESPACIO = ' ';
		public const char		CERO    = '0';
		public const int		DELANTE = 0;
		public const int		DETRAS  = 1;

		#endregion

		#region EnteroSQL

		/// <summary>
		/// Devuelve un número SQL sin separador de miles
		/// </summary>
		/// <param name="VAL">Valor donde quitar el separador</param>
		/// <returns>Valor sin separado</returns>
		public static String EnteroSQL(String VAL)
		{
			String RET = "";

			for (int i = 0; i < VAL.Length; i++)
			{
				if ((VAL[i] != ',') && (VAL[i] != '.'))
				{
					RET += VAL[i];
				}
			}

			if (RET.Trim() == "")
				RET = "0";

			return RET;
		}

		#endregion

		#region AppPath

		/// <summary>
		/// Devuelve el path de la aplicación
		/// </summary>
		/// <returns>Path aplicación</returns>
		public static String AppPath()
		{
			return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\";
		}
		
		#endregion

		#region AppName

		/// <summary>
		/// Devueelve el nombre de la aplicación
		/// </summary>
		/// <returns>Nombre aplicación</returns>
		public static String AppName()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
		}
		
		#endregion

		#region FechaHoraSQL

		/// <summary>
		/// Devuelve una cadena con la fecha y hora aceptada por SQL Server
		/// </summary>
		/// <param name="fecha">Fecha a formatear</param>
		/// <returns>Fecha formateada</returns>
		public static String FechaHoraSQL(DateTime fecha)
		{
			String SQLFecha;

			if (fecha != new DateTime(0))
			{
				SQLFecha = fecha.ToString("dd/MM/yyyy HH:mm:ss");
				SQLFecha = "'" + SQLFecha + "'";
			}
			else
			{
				SQLFecha = "NULL";
			}
			return (SQLFecha);
		}

		#endregion

		#region FechaSQL

		/// <summary>
		/// Devuelve una cadena con la fecha aceptada por SQL Server
		/// </summary>
		/// <param name="fecha">Fecha a formatear</param>
		/// <returns>Fecha formateada</returns>
		public static String FechaSQL(DateTime fecha)
		{
			String SQLFecha;

			if (fecha != new DateTime(0))
			{
				SQLFecha = fecha.ToString("dd/MM/yyyy");
				SQLFecha = "'" + SQLFecha + "'";
			}
			else
			{
				SQLFecha = "NULL";
			}
			return (SQLFecha);
		}

		#endregion

		#region InsertText

		/// <summary>
		/// Formatea una cadena de texto
		/// </summary>
		/// <param name="text">Testo original</param>
		/// <param name="car">Carácter de relleno</param>
		/// <param name="len">Longitud final</param>
		/// <param name="pos">Posición relleno DERECHA - IZQUIERDA</param>
		/// <returns>Cadena formateada</returns>
		public static String InsertText(String text, char car, int len, int pos)
		{
			String tmp = "";
			int l;

			l = text.Length;
			if (l >= len)
				text = text.Substring(0, len);
			else
			{
				while (tmp.Length < (len - l))
					tmp = tmp + car;
			}

			if (pos == DELANTE)
				text = tmp + text;
			else
				text = text + tmp;

			return (text);
		}

		#endregion

		#region Cifrar

		/// <summary>
		/// Cifrar un texto hasta una longitud
		/// </summary>
		/// <param name="password">Cadena original</param>
		/// <param name="len">Longitud</param>
		/// <returns>Cadena cifrada</returns>
		public static String Cifrar(String password, int len)
		{
			String PassCifrado = "";
			char bit;
			int contarComilla = 0;

			password = InsertText(password, ESPACIO, len, DETRAS);
			for (int i = 1; i <= len; i++)
			{
				bit = (char)((int)password[i - 1] ^ (32 - i));

				if (bit == '\'')
					contarComilla++;
				else if (contarComilla > 0)
				{
					if (contarComilla % 2 != 0)
						PassCifrado = PassCifrado + '\'';

					contarComilla = 0;
				}

				PassCifrado = PassCifrado + bit;
			}

			return PassCifrado;
		}

		#endregion

		#region BooleanSQL

		/// <summary>
		/// Devuelve una cadena con  0 - false / 1 - true
		/// </summary>
		/// <param name="valor">Valor a convertir</param>
		/// <returns>"0" - "1"</returns>
		public static String BooleanSQL(bool valor)
		{
			if (valor)
			{
				return ("1");
			}
			else
			{
				return ("0");
			}
		}

		#endregion

		#region BooleanSQL

		/// <summary>
		/// Devuelve una cadena con  0 - false / 1 - true
		/// </summary>
		/// <param name="valor">Valor a convertir</param>
		/// <returns>"0" - "1"</returns>
		public static String BooleanSQL(String valor)
		{
			if (valor == "True")
			{
				return ("1");
			}
			else
			{
				return ("0");
			}
		}

		#endregion

		#region StrTalkman

		/// <summary>
		/// Filtro de las podibles , que aparezcan en los parametros
		/// </summary>
		/// <param name="Cadena">Cadena a filtrar</param>
		/// <returns>Cadena filtrada</returns>
		public static String StrTalkman(String Cadena)
		{
			int         pos_apos;
			String			FilteredCadena, CadAux;

			// filtramos la coma ,
			CadAux          = Cadena;
			FilteredCadena  = "";
			pos_apos        = CadAux.IndexOf(",");
			while (pos_apos >= 0) 
			{
				FilteredCadena	= FilteredCadena + CadAux.Substring(0, pos_apos);
				CadAux					= CadAux.Substring(pos_apos + 1);
				pos_apos				= CadAux.IndexOf(",");
			}
			FilteredCadena = FilteredCadena + CadAux;

			return (FilteredCadena);
		}

		#endregion

		#region StrSQL

		/// <summary>
		/// Filtro de los posibles ' que aparezcan en los parametros
		/// </summary>
		/// <param name="Cadena">Cadena a filtrar</param>
		/// <returns>Cadena filtrada</returns>
		public static String StrSQL(String Cadena)
		{
			int         pos_apos;
			String			FilteredCadena, CadAux;

			// filtramos comilla '
			CadAux          = Cadena;
			FilteredCadena  = "";
			pos_apos        = CadAux.IndexOf("'");
			while (pos_apos >= 0) 
			{
				FilteredCadena	= FilteredCadena + CadAux.Substring(0, pos_apos + 1) + "'";
				CadAux					= CadAux.Substring(pos_apos + 1);
				pos_apos				= CadAux.IndexOf("'");
			}
			FilteredCadena	= FilteredCadena + CadAux;

			// filtramos comilla `
			CadAux          = FilteredCadena;
			FilteredCadena  = "";
			pos_apos        = CadAux.IndexOf("`");
			while (pos_apos >= 0) 
			{
				FilteredCadena	= FilteredCadena + CadAux.Substring(0, pos_apos);
				CadAux					= CadAux.Substring(pos_apos + 1);
				pos_apos				= CadAux.IndexOf("`");
			}
			FilteredCadena = FilteredCadena + CadAux;

			// filtramos comilla ´
			CadAux          = FilteredCadena;
			FilteredCadena  = "";
			pos_apos        = CadAux.IndexOf("´");
			while (pos_apos >= 0) 
			{
				FilteredCadena	= FilteredCadena + CadAux.Substring(0, pos_apos);
				CadAux					= CadAux.Substring(pos_apos + 1);
				pos_apos				= CadAux.IndexOf("´");
			}
			FilteredCadena = FilteredCadena + CadAux;

			// filtramos comilla "
			CadAux          = FilteredCadena;
			FilteredCadena  = "";
			pos_apos        = CadAux.IndexOf("\"");
			while (pos_apos >= 0) 
			{
				FilteredCadena	= FilteredCadena + CadAux.Substring(0, pos_apos);
				CadAux					= CadAux.Substring(pos_apos + 1);
				pos_apos				= CadAux.IndexOf("\"");
			}
			FilteredCadena = FilteredCadena + CadAux;

			return (FilteredCadena);
		}

		#endregion

		#region NumToInt

		/// <summary>
		/// Elimina los separadores de miles y la parte decimal de la cadena
		/// </summary>
		/// <param name="txt">Cadena a filtrar</param>
		/// <returns>Cadena filtrada</returns>
		public static String NumToInt(String txt)
		{
			String    n_txt = "";

			for (int i = 0; i < txt.Length; i++) 
			{
				if (txt[i].ToString() == Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) 
					break;
				else 
				{
					if ((txt[i] != ',') && (txt[i] != '.')) 
						n_txt = n_txt + txt[i];
				}
			}
  
			return (n_txt);
		}

		#endregion

		#region EsNumeroEstero

		/// <summary>
		/// Comprueba si la cadena pasada es un número entero
		/// </summary>
		/// <param name="Str">Cadena a convertir</param>
		/// <returns>True - Es número entero / False - No es número entero</returns>
		public static bool EsNumeroEstero(String Str)
		{
			bool RET = false;

			try
			{
				Int64.Parse(Str);
				RET = true;
			}
			catch
			{

			}
			return (RET);
		}

		#endregion

    #region CharStringToString

		/// <summary>
		/// Convierte una cadena que puede contener representaciones
		/// de caracteres (0x02...) a un String
		/// </summary>
		/// <param name="CharString">Cadena de entrada</param>
		/// <returns>Cadena de salida</returns>
		public static String CharStringToString(String CharString)
		{ 
			String	cadena = "";
			String	hex = "";
			Char		c;
			int			pos = 0;

			while (CharString.Length > 0)
			{
				pos = CharString.IndexOf("<0x");

				if (pos >= 0)
				{
					cadena		+= CharString.Substring(0, pos);
					CharString = CharString.Substring(pos);

					pos = CharString.IndexOf(">");

					if (pos == 5)
					{
						hex				= CharString.Substring(3,2);
						c					= (Char)int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
						cadena		+= c.ToString();
					}
					else
						cadena		+= CharString.Substring(0, pos);
					
					CharString = CharString.Substring(pos + 1);
				}
				else
				{
					cadena		+= CharString;
					CharString = "";
				}
			}
			return cadena;
		}

		#endregion

	#endregion
	}
}
