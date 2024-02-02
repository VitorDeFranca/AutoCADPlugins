using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.Xml.Linq;


namespace UserInteraction
{
    public class UserInteractionsClass
    {
        [CommandMethod("GetName")]
        public void GetNameUsingGetString()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor edt = doc.Editor;

            PromptStringOptions prompt = new PromptStringOptions("Enter your name: ");
            prompt.AllowSpaces = true;

            PromptResult result = edt.GetString(prompt);

            if (result.Status == PromptStatus.OK)
            {
                string name = result.StringResult;
                edt.WriteMessage($"Hello there {name}!");
                Application.ShowAlertDialog($"Your name is {name}");
            }
            else
            {
                edt.WriteMessage($"No name entered.");
                Application.ShowAlertDialog($"No name entered.");
            }
        }

        [CommandMethod("SetLayerUsingGetString")]
        public void SetLayerUsingGetString()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                
                PromptStringOptions promptInput = new PromptStringOptions("Enter layer to make current: ");
                promptInput.AllowSpaces = false;

                //Get the results of the user input
                PromptResult promptResult = edt.GetString(promptInput);
                if (promptResult.Status == PromptStatus.OK)
                {
                    string layerName = promptResult.StringResult;

                    //Check if the emtered layer name exist in the layer database
                    if (lyTab.Has(layerName))
                    {
                        db.Clayer = lyTab[layerName];

                        trans.Commit();
                    }
                    else
                    {
                        Application.ShowAlertDialog($"The layer {layerName} does not exist in the project.");
                    }

                }
                else
                {
                    Application.ShowAlertDialog("No layer entered");
                }

            }

        }

        [CommandMethod("CreateLineUsingGetPoint")]
        public void CreateLineUsingGetPoint()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            //Prompt for the starting point
            PromptPointOptions ppo = new PromptPointOptions("Pick starting point: ");
            PromptPointResult ppr = edt.GetPoint(ppo);
            Point3d startPt = ppr.Value;

            //Prompt for the end point and specify the startpoint as the basepoint
            ppo = new PromptPointOptions("Pick end point: ");
            ppo.UseBasePoint = true;
            ppo.BasePoint = startPt;
            ppr = edt.GetPoint(ppo);
            Point3d endPt = ppr.Value;

            if (startPt == null || endPt == null)
            {
                edt.WriteMessage("Invalid point");
                return;
            }
            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Line ln = new Line(startPt, endPt);
                ln.SetDatabaseDefaults();

                btr.AppendEntity(ln);
                trans.AddNewlyCreatedDBObject(ln, true);

                trans.Commit();

            }

        }

        [CommandMethod("CreateDistanceBetweenTwoPoints")]
        public void CreateDistanceBetweenTwoPoints()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor edt = doc.Editor;
            PromptDoubleResult pdr; pdr = edt.GetDistance("Pick two points to get the distance");

            Application.ShowAlertDialog($"\nDistance between points: {pdr.Value}");

            

        }

        [CommandMethod("DrawObjectUsingGetKeyWords")]
        public void DrawObjectUsingGetKeyWords()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            PromptKeywordOptions pko = new PromptKeywordOptions("");
            pko.Message = "\nWhat would you like to draw?";
            pko.Keywords.Add("line");
            pko.Keywords.Add("circle");
            pko.Keywords.Add("mtext");
            pko.AllowNone = false;

            PromptResult res = doc.Editor.GetKeywords(pko);
            string answer = res.StringResult.ToLower();
            if (answer != null)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    switch (answer) 
                    {
                        case "line":
                            Point3d pt1 = new Point3d(0, 0, 0);
                            Point3d pt2 = new Point3d(100, 100, 0);
                            Line ln = new Line(pt1,pt2);
                            btr.AppendEntity(ln);
                            trans.AddNewlyCreatedDBObject(ln, true);
                            break;
                        case "circle":
                            Point3d cenPt = new Point3d(0, 0, 0);
                            Circle cir = new Circle();
                            cir.Center = cenPt;
                            cir.Radius = 10;
                            cir.ColorIndex = 1;
                            btr.AppendEntity(cir);
                            trans.AddNewlyCreatedDBObject(cir, true);
                            break;
                        case "mtext":
                            Point3d insPt = new Point3d(0, 0, 0);
                            MText mtx = new MText();
                            mtx.Contents = "Hello World!";
                            mtx.Location = insPt;
                            mtx.TextHeight = 10;
                            mtx.ColorIndex = 2;
                            btr.AppendEntity(mtx);
                            trans.AddNewlyCreatedDBObject(mtx, true);
                            break;
                        default:
                            doc.Editor.WriteMessage("No option selected");
                            break;
                    }


                    trans.Commit();

                }
            }


            



        }
    }
}
