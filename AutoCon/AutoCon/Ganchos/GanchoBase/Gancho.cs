using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCon.Ganchos.GanchoBase
{
    public class Gancho
    {

        //Medidas em centimetros
        public double Diametro;
        public double Prolongamento;
        public double ComprAdicional;

        public Gancho(double diametro, double prolongamento, double comprAdicional)
        {
            Diametro = diametro;
            Prolongamento = prolongamento;
            ComprAdicional = comprAdicional;
        }


    }
}
