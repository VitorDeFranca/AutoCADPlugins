using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using TitleBlocks.Model;
using TitleBlocks.Sizes;

namespace TitleBlocks
{
    public class DrawTBlock
    {
        [CommandMethod("SELOA4")]
        public void DesenharA4()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            
            TBlock tb = new TBlock();
            A4TBlock a4 = new A4TBlock();

            tb.DesenharSelo(a4.Height, a4.Width);
        }

        [CommandMethod("SELOA3")]
        public void DesenharA3()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            
            TBlock tb = new TBlock();
            A3TBlock a3 = new A3TBlock();

            tb.DesenharSelo(a3.Height, a3.Width);
        }

        [CommandMethod("SELOA2")]
        public void DesenharA2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            
            TBlock tb = new TBlock();
            A2TBlock a2 = new A2TBlock();

            tb.DesenharSelo(a2.Height, a2.Width);
        }

        [CommandMethod("SELOA1")]
        public void DesenharA1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            
            TBlock tb = new TBlock();
            A1TBlock a1 = new A1TBlock();

            tb.DesenharSelo(a1.Height, a1.Width);
        }

        [CommandMethod("SELOA0")]
        public void DesenharA0()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            
            TBlock tb = new TBlock();
            A0TBlock a0 = new A0TBlock();

            tb.DesenharSelo(a0.Height, a0.Width);
        }
    }
}
