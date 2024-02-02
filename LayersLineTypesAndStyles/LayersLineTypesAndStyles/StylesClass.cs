using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;


namespace LayersLineTypesAndStyles
{
    public class StylesClass
    {
        [CommandMethod("ListStyles")]
        public static void ListStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTable stTab = trans.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;

                foreach (ObjectId stId in stTab)
                {
                    TextStyleTableRecord stRec = trans.GetObject(stId, OpenMode.ForRead) as TextStyleTableRecord;
                    doc.Editor.WriteMessage($"\nStyle name: {stRec.Name}");
                }
                trans.Commit();

            }
        }

        [CommandMethod("UpdateCurrentTextStyleFont")]
        public static void UpdateCurrentTextStyleFont()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTableRecord stRec = trans.GetObject(db.Textstyle, OpenMode.ForWrite) as TextStyleTableRecord;

                FontDescriptor font = stRec.Font;

                FontDescriptor newFont = new FontDescriptor("ARCHITECT", font.Bold, font.Italic, font.CharacterSet, font.PitchAndFamily);
                stRec.Font = newFont;

                doc.Editor.Regen();

                trans.Commit();

            }
        }

        [CommandMethod("SetCurrentTextStyle")]
        public static void SetCurrentTextStyle()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTable stTab = trans.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;
                foreach (ObjectId stId in stTab)
                {
                    TextStyleTableRecord stRec = trans.GetObject(stId, OpenMode.ForRead) as TextStyleTableRecord;
                    if (stRec.Name == "ARCHITECT")
                    {
                        Application.SetSystemVariable("TEXTSTYLE", "ARCHITECT");
                        doc.Editor.WriteMessage($"\nStyle name {stRec.Name} is now the default TextStyle.");

                        trans.Commit();
                        break;
                    }

                }
            }
        }

        [CommandMethod("SetTextStyleToObject")]
        public static void SetTextStyleToObject()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt;
                bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord btr;
                btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                using (MText mtx = new MText())
                {
                    Point3d insPt = new Point3d(0, 0, 0);
                    mtx.Contents = "Hello AutoCAD";
                    mtx.TextHeight = 9;
                    mtx.Location = insPt;

                    mtx.TextStyleId = db.Textstyle;

                    btr.AppendEntity(mtx);
                    trans.AddNewlyCreatedDBObject(mtx, true);

                    trans.Commit();

                }
            }
        }
    }
}
