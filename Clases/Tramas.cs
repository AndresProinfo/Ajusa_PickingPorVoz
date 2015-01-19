using System;
using System.Collections.Generic;
using System.Text;


namespace ServicioPPV
{
    class Tramas
    {
        #region VbleLocal
            private string TipoTrama;
            private string ProcAlmacenado;
            string[,] Campos = new string[15,2];

        #endregion

        #region Metodos
            public Tramas(string trama)
            {
                char[] DelimitadorCadena = { ';' };
                string[] Campos = trama.Split(DelimitadorCadena);

                TipoTrama = Campos[2];
            }

            
  
        #endregion
    }
}
