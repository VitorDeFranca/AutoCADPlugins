using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;


namespace HelloAutoCAD
{
    public class Class1
    {
        [CommandMethod("HelloAutoCAD")]
        public void HelloAutoCADFromCSharp()
        {
            //Instantiating the active drawing
            Document doc = Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;
            Editor edt = doc.Editor;

            edt.WriteMessage("Hello AUtoCAD from C#!");
        }

        [CommandMethod("SayHi")]
        public void SayHi()
        {
            Application.ShowAlertDialog("Hello AutoCAD from C#!2");
        }
    }
}
