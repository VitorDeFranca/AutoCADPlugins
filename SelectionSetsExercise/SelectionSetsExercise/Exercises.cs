using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace SelectionSetsExercise
{
    public class Exercises
    {
        [CommandMethod("Exercise1")]
        public void Exercise1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            TypedValue[] tv = new TypedValue[3];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
            tv.SetValue(new TypedValue((int)DxfCode.BlockName, "Receptacles"), 1);
            tv.SetValue(new TypedValue((int)DxfCode.LayerName, "Power"), 2);


            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} receptacles selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }

        [CommandMethod("Exercise2")]
        public void Exercise2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            edt.WriteMessage("\nSelecting all the Lighting Fixtures in the drawing");

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                TypedValue[] tv = new TypedValue[3];
                tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
                tv.SetValue(new TypedValue((int)DxfCode.BlockName, "Lighting Fixture"), 1);
                tv.SetValue(new TypedValue((int)DxfCode.LayerName, "Lighting"), 2);


                SelectionFilter filter = new SelectionFilter(tv);

                PromptSelectionResult psr = edt.SelectAll(filter);

                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;
                    foreach (SelectedObject sObj in ss)
                    {
                        if (sObj != null)
                        {
                            Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;
                            ent.Layer = "Power";
                        }
                    }
                    edt.WriteMessage($"There are a total of {ss.Count} receptacles selected");
                }
                else
                {
                    edt.WriteMessage("No object selected");
                }
            } 
        }
    } 
}
