using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCon.Estribos.EstriboBase
{
    public class Estribo
    {
        public double Diametro;
        public double Prolongamento;
        public double ComprAdicional;

        public Estribo(double diametro, double prolongamento, double comprAdicional)
        {
            Diametro = diametro;
            Prolongamento = prolongamento;
            ComprAdicional = comprAdicional;
        }
    }
}
