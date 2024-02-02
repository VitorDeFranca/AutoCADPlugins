using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace SelectionSets
{
    public class SelectionSetsClass
    {
        [CommandMethod("SelectAllAndChangeLayer")]
        public void SelectAllAndChangeLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionResult psr = edt.SelectAll();
                SelectionSet ss = psr.Value;

                if (psr.Status == PromptStatus.OK)
                {
                    foreach (SelectedObject sObj in ss) 
                    {
                        if (sObj != null)
                        {
                            Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;
                            ent.Layer = "Cabinetry";
                        }
                    }
                    trans.Commit();
                }
            }
        }

        [CommandMethod("SelectObjectOnScreen")]
        public void SelectObjectOnScreen()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionResult psr = edt.GetSelection();
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;
                    edt.WriteMessage($"\nThere are a total of {ss.Count} objects selected.");

                    foreach(SelectedObject sObj in ss)
                    {
                        Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {
                            ent.ColorIndex = 1;
                        }
                    }
                }
                trans.Commit();
                
            }
        }

        [CommandMethod("SelectWindowAndChangeColor")]
        public void SelectWindowAndChangeColor()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionResult psr = edt.SelectWindow(new Point3d(0, 0, 0), new Point3d(500, 500, 0));
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;
                    edt.WriteMessage($"\nThere are a total of {ss.Count} objects selected.");

                    foreach (SelectedObject sObj in ss)
                    {
                        Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {
                            ent.ColorIndex = 1;
                        }
                    }
                }
                else
                {
                    edt.WriteMessage("No objected selected.");
                }

                trans.Commit();

            }
        }

        [CommandMethod("SelectCrossingWindowAndDelete")]
        public void SelectCrossingWindowAndDelete()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                PromptSelectionResult psr = edt.SelectCrossingWindow(new Point3d(0, 0, 0), new Point3d(500, 500, 0));
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;
                    edt.WriteMessage($"\nThere are a total of {ss.Count} objects selected.");

                    foreach (SelectedObject sObj in ss)
                    {
                        if (sObj != null)
                        {
                            Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;  
                            if (ent != null)
                                ent.Erase(true);
                        }
                    }
                }
                else
                {
                    edt.WriteMessage("No objected selected.");
                }

                trans.Commit();

            }
        }

        [CommandMethod("SelectFenceAndChangeLayer")]
        public void SelectFenceAndChangeLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                Point3dCollection p3dcol = new Point3dCollection();
                p3dcol.Add(new Point3d(0, 0, 0));
                p3dcol.Add(new Point3d(500, 300, 0));
                PromptSelectionResult psr = edt.SelectFence(p3dcol);

                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;
                    
                    foreach (SelectedObject sObj in ss)
                    {
                        if (sObj != null)
                        {
                            Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;
                            if (ent != null)
                                ent.Layer = "Misc";
                        }
                    }
                }
                else
                {
                    edt.WriteMessage("No objected selected.");
                }

                trans.Commit();

            }
        }

        [CommandMethod("PickFirstSelection", CommandFlags.UsePickSet)]
        public void PickFirstSelection()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor edt = doc.Editor;

            PromptSelectionResult psr = edt.SelectImplied();

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                Application.ShowAlertDialog($"\nNumber of objects in PickFirst selection: {ss.Count}");
            }
            else
            {
                Application.ShowAlertDialog($"No object selected");

            }

            ObjectId[] ids = new ObjectId[0];
            edt.SetImpliedSelection(ids);

            psr = edt.GetSelection();

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                Application.ShowAlertDialog($"\nNumber of objects in selected: {ss.Count}");
            }
            else
            {
                Application.ShowAlertDialog($"No object selected");

            }

        }

        [CommandMethod("SelectLines")]
        public void SelectLines()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                edt.WriteMessage("Selecting all the line objects");

                TypedValue[] tv = new TypedValue[1];
                tv.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);
                SelectionFilter filter = new SelectionFilter(tv);

                PromptSelectionResult psr = edt.SelectAll(filter);

                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;
                    foreach (SelectedObject sObj in ss)
                    {
                        Entity ent = trans.GetObject(sObj.ObjectId, OpenMode.ForWrite) as Entity;
                        ent.ColorIndex = 1;

                    }

                    edt.WriteMessage($"There are a total of {ss.Count} selected");
                }

                trans.Commit();
            }
        }

        [CommandMethod("SelectMTexts")]
        public void SelectMTexts()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;
            
            TypedValue[] tv = new TypedValue[1];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "MTEXT"), 0);

            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} MTexts selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }

        [CommandMethod("SelectPlines")]
        public void SelectPlines()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            TypedValue[] tv = new TypedValue[1];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "LWPOLYLINE"), 0);

            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} LWPolylines selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }

        [CommandMethod("SelectFrenchDoors")]
        public void SelectFrenchDoors()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            TypedValue[] tv = new TypedValue[2];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
            tv.SetValue(new TypedValue((int)DxfCode.BlockName, "Door - French"), 1);


            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} French-Doors selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }

        [CommandMethod("SelectBiFold")]
        public void SelectBiFold()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            TypedValue[] tv = new TypedValue[2];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
            tv.SetValue(new TypedValue((int)DxfCode.BlockName, "Door - Bifold"), 1);


            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} BiFold-Doors selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }

        [CommandMethod("SelectWalls")]
        public void SelectWalls()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            TypedValue[] tv = new TypedValue[2];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "LWPOLYLINE"), 0);
            tv.SetValue(new TypedValue((int)DxfCode.LayerName, "Walls"), 1);


            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} Walls selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }

        [CommandMethod("SelectStairs")]
        public void SelectStairs()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edt = doc.Editor;

            TypedValue[] tv = new TypedValue[2];
            tv.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);
            tv.SetValue(new TypedValue((int)DxfCode.LayerName, "Stairs"), 1);


            SelectionFilter filter = new SelectionFilter(tv);

            PromptSelectionResult psr = edt.SelectAll(filter);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                edt.WriteMessage($"There are a total of {ss.Count} Stairs selected");
            }
            else
            {
                edt.WriteMessage("No object selected");
            }
        }
    }
}

