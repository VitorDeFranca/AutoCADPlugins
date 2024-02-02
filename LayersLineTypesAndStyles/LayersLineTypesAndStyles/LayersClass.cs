using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.Geometry;

namespace LayersLineTypesAndStyles
{
    public class LayersClass
    {
        [CommandMethod("ListLayers")]
        public static void ListLayers()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                foreach (ObjectId lyId in lyTab)
                {
                    LayerTableRecord lyRec = trans.GetObject(lyId, OpenMode.ForRead) as LayerTableRecord;
                    doc.Editor.WriteMessage($"\nLayer name: {lyRec.Name}");
                }

                trans.Commit();
            }
        }

        [CommandMethod("CreateLayer")]
        public static void CreateLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                if (lyTab.Has("Misc"))
                {
                    doc.Editor.WriteMessage("Layer already exist.");
                    trans.Abort();
                }
                else
                {
                    lyTab.UpgradeOpen();
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = "Misc";
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByLayer, 1);

                    lyTab.Add(ltr);
                    trans.AddNewlyCreatedDBObject(ltr, true);

                    db.Clayer = lyTab["Misc"];
                    doc.Editor.WriteMessage($"Layer {ltr.Name} was created successfully.");
                    trans.Commit();
                }

            }
        }

        [CommandMethod("UpdateLayer")]
        public static void UpdateLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    foreach (ObjectId lyId in lyTab)
                    {
                        LayerTableRecord lyRec = trans.GetObject(lyId, OpenMode.ForRead) as LayerTableRecord;
                        if (lyRec.Name == "Misc")
                        {
                            lyRec.UpgradeOpen();

                            //Update the color
                            lyRec.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);

                            //Update the linetype
                            LinetypeTable ltTab = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                            if (ltTab.Has("Hidden"))
                            {
                                lyRec.LinetypeObjectId = ltTab["Hidden"];
                            }

                            trans.Commit();
                            doc.Editor.WriteMessage($"\nCompleted updating Layer {lyRec.Name}");
                            break;
                        }
                        else
                        {
                            doc.Editor.WriteMessage($"\nSkipping Layer {lyRec.Name}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }
                
                

            }
        }

        [CommandMethod("SetLayerOnOff")]
        public static void SetLayerOnOff()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    db.Clayer = lyTab["0"];

                    foreach (ObjectId lyId in lyTab)
                    {
                        LayerTableRecord lyRec = trans.GetObject(lyId, OpenMode.ForRead) as LayerTableRecord;
                        if (lyRec.Name == "Misc")
                        {
                            lyRec.UpgradeOpen();

                            //Turn the layer ON or OFF
                            lyRec.IsOff = true;

                            trans.Commit();
                            doc.Editor.WriteMessage($"\nLayer {lyRec.Name} has been turned off");
                            break;
                        }
                        else
                        {
                            doc.Editor.WriteMessage($"\nSkipping Layer {lyRec.Name}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }

            }
        }

        [CommandMethod("SetLayerFrozenOrThaw")]
        public static void SetLayerFrozenOrThaw()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    db.Clayer = lyTab["0"];

                    foreach (ObjectId lyId in lyTab)
                    {
                        LayerTableRecord lyRec = trans.GetObject(lyId, OpenMode.ForRead) as LayerTableRecord;
                        if (lyRec.Name == "Misc")
                        {
                            lyRec.UpgradeOpen();

                            //Turn the layer ON or OFF
                            lyRec.IsFrozen = true;

                            trans.Commit();
                            doc.Editor.WriteMessage($"\nLayer {lyRec.Name} has been frozen");
                            break;
                        }
                        else
                        {
                            doc.Editor.WriteMessage($"\nSkipping Layer {lyRec.Name}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }

            }
        }

        [CommandMethod("DeleteLayer")]
        public static void DeleteLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    db.Clayer = lyTab["0"];

                    foreach (ObjectId lyId in lyTab)
                    {
                        LayerTableRecord lyRec = trans.GetObject(lyId, OpenMode.ForRead) as LayerTableRecord;
                        if (lyRec.Name == "Misc")
                        {
                            lyRec.UpgradeOpen();

                            //Turn the layer ON or OFF
                            lyRec.Erase();

                            trans.Commit();
                            doc.Editor.WriteMessage($"\nLayer {lyRec.Name} has been deleted");
                            break;
                        }
                        else
                        {
                            doc.Editor.WriteMessage($"\nSkipping Layer {lyRec.Name}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }

            }
        }

        [CommandMethod("LockUnlockLayer")]
        public static void LockUnlockLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    LayerTable lyTab = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    db.Clayer = lyTab["0"];

                    foreach (ObjectId lyId in lyTab)
                    {
                        LayerTableRecord lyRec = trans.GetObject(lyId, OpenMode.ForRead) as LayerTableRecord;
                        if (lyRec.Name == "Misc")
                        {
                            lyRec.UpgradeOpen();

                            //Turn the layer ON or OFF
                            lyRec.IsLocked = true; ;

                            trans.Commit();
                            doc.Editor.WriteMessage($"\nLayer {lyRec.Name} has been locked");
                            break;
                        }
                        else
                        {
                            doc.Editor.WriteMessage($"\nSkipping Layer {lyRec.Name}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }

            }
        }

        [CommandMethod("SetLayerToObject")]
        public static void SetLayerToObject()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt;
                    bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord btr;
                    btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Point3d pt1 = new Point3d(0, 0, 0);
                    Point3d pt2 = new Point3d(100, 100, 0);
                    Line line = new Line(pt1, pt2);

                    //Assign a layer to the object

                    line.Layer = "Cabinetry";

                    btr.AppendEntity(line);
                    trans.AddNewlyCreatedDBObject(line, true);

                    trans.Commit();
                    doc.Editor.WriteMessage($"New line object was added to {line.Layer} layer");


                   
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
                    trans.Abort();
                }

            }
        }
    }
}
