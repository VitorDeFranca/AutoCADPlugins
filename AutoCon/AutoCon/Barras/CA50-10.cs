using AutoCon.Barras.BarraBase;
using AutoCon.Estribos.EstriboBase;
using AutoCon.Ganchos.GanchoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCon.Barras
{
    public class CA50_10 : Barra
    {
        //Ganchos 
        Gancho gancho90 = new Gancho(5.0,8.0,20.0);
        Gancho gancho135 = new Gancho(5.0,4.0,20.0);
        Gancho gancho180 = new Gancho(5.0,2.0,20.0);

        Estribo estribo90 = new Estribo(3.0, 10.0, 25.0);
        Estribo estribo135 = new Estribo(3.0, 5.0, 25.0);
        Estribo estribo180 = new Estribo(3.0, 5.0, 25.0);
        
        public CA50_10()
        {
           
                
        }



    }
}
