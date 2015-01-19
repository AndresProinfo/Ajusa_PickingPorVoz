using System;
using System.Collections.Generic;
using System.Text;

namespace ServicioPPV
{
    class ParametrosPA
    {
        //array para pasar los parametros, tipos y valor a los PA
        private string[,] Parametros = new string[20, 3];
        private int NumParametros;



        public ParametrosPA(String Cad)
        {
            char[] DelimitadorCadena = { ';' };
            string[] Campos = Cad.Split(DelimitadorCadena);


        }



    }
}
