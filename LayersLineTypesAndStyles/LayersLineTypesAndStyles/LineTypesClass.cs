using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

namespace LayersLineTypesAndStyles
{
    public class LineTypesClass
    {
        [CommandMethod("ListLineTypes")]
        public static void ListLineTypes()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LinetypeTable ltTab = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;

                foreach (ObjectId ltId in ltTab)
                {
                    LinetypeTableRecord ltRec = trans.GetObject(ltId, OpenMode.ForRead) as LinetypeTableRecord;
                    doc.Editor.WriteMessage($"\nLinetype: {ltRec.Name}");
                }

                trans.Commit();

                

            }
        }

        [CommandMethod("LoadLineType")]
        public static void LoadLineType()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {

                try
                {
                    LinetypeTable ltTab = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    string ltName = "CENTER";
                    if (ltTab.Has(ltName))
                    {
                        doc.Editor.WriteMessage($"\nLinetype aleady exist");
                        trans.Abort();
                    }
                    else
                    {
                        db.LoadLineTypeFile(ltName, "acad.lin");
                        doc.Editor.WriteMessage($"LineType {ltName} was created successfully");
                        trans.Commit();
                    }

                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }

            }
        }

        [CommandMethod("SetCurrentLineType")]
        public static void SetCurrentLineType()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            string ltName = "DASHED2";
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LinetypeTable ltTab = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                if (ltTab.Has(ltName))
                {
                    db.Celtype = ltTab[ltName];
                    doc.Editor.WriteMessage($"\nLinetype {db.Celtype} is now the current linetype.");
                    trans.Commit();
                }

            }
        }

        [CommandMethod("DeleteLineType")]
        public static void DeleteLineType()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    LinetypeTable ltTab = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    db.Celtype = ltTab["ByLayer"];

                    foreach(ObjectId ltId in ltTab)
                    {
                        LinetypeTableRecord ltRec = trans.GetObject(ltId, OpenMode.ForRead) as LinetypeTableRecord;
                        
                        if (ltRec.Name == "DASHED2")
                        {
                            ltRec.UpgradeOpen();
                            ltRec.Erase(true);

                            trans.Commit();
                            doc.Editor.WriteMessage("\nLinetype deleted successfully");
                            break;
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"\nError encountered: {ex.Message}");
                    trans.Abort();
                    
                }

            }
        }

        [CommandMethod("SetLineTypeToObject")]
        public static void SetLineTypeToObject()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt;
                bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord btr;
                btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Point3d pt1 = new Point3d(0, 0, 0);
                Point3d pt2 = new Point3d(100, 100, 0);
                Line line = new Line(pt1, pt2);

                //Set the Linetype
                line.Linetype = "HIDDEN";

                btr.AppendEntity(line);
                trans.AddNewlyCreatedDBObject(line, true);

                trans.Commit();

            }
        }
    }
}
