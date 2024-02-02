using AutoCon.Ganchos.GanchoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCon.Ganchos
{
    public class Gancho180 : Gancho
    {
        public Gancho180(double diametro) 
        {
            Diametro = 5.0;
            Prolongamento = 2.0;
            ComprAdicional = 20.0;
        }
    }
}
