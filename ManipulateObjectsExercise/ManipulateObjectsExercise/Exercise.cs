using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;

namespace ManipulateObjectsExercise
{
    public class Exercise
    {
        [CommandMethod("COPYX")]
        public static void CopyExercise()
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

					using (Circle c1 = new Circle())
					{
						c1.Radius = 1;
						c1.Center = new Point3d(0, 0, 0);
						c1.ColorIndex = 1;

						btr.AppendEntity(c1);
						trans.AddNewlyCreatedDBObject(c1, true);

						Circle c2 = new Circle();
						c2.Radius = 2;
						c2.Center = new Point3d(10, 10, 0);

						btr.AppendEntity(c2);
						trans.AddNewlyCreatedDBObject(c2, true);

						Circle c3 = new Circle();
						c3.Radius = 5;
						c3.Center = new Point3d(30, 30, 0);
						c3.ColorIndex = 5;

						btr.AppendEntity(c3);
						trans.AddNewlyCreatedDBObject(c3, true);

						DBObjectCollection collection = new DBObjectCollection();
						collection.Add(c1);
						collection.Add(c2);
						collection.Add(c3);

						foreach (Circle circle in collection)
						{
							if (circle.Radius == 2)
							{
								Circle c4 = circle.Clone() as Circle;
								c4.ColorIndex = 3;
								c4.Radius = 10;

								btr.AppendEntity(c4);
								trans.AddNewlyCreatedDBObject(c4, true);


							}

						}

					}
					trans.Commit();
				}
                catch (System.Exception ex)
                {

					doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
					trans.Abort();
				}
            }

        }

		[CommandMethod("ERASEX")]
		public static void EraseExercise()
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

					using (Line line = new Line())
					{
						line.StartPoint = new Point3d(0, 0, 0);
						line.EndPoint = new Point3d(10, 10, 0);

						btr.AppendEntity(line);
						trans.AddNewlyCreatedDBObject(line, true);

						Circle circle = new Circle();
						circle.Center = new Point3d(0, 0, 0);
						circle.Radius = 5;

						btr.AppendEntity(circle);
						trans.AddNewlyCreatedDBObject(circle, true);

                        Autodesk.AutoCAD.DatabaseServices.Polyline polyline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
						polyline.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
						polyline.AddVertexAt(1, new Point2d(-10, -10), 0, 0, 0);
						polyline.AddVertexAt(2, new Point2d(20, -20), 0, 0, 0);

						btr.AppendEntity(polyline);
						trans.AddNewlyCreatedDBObject(polyline, true);

						DBObjectCollection collection = new DBObjectCollection();
						collection.Add(line);
						collection.Add(circle);
						collection.Add(polyline);

						foreach (Entity item in collection)
						{
							if (item is Line)
							{
								item.ColorIndex = 2;
							}
							else if (item is Autodesk.AutoCAD.DatabaseServices.Polyline)
							{
								item.ColorIndex = 3;
							}
							else
							{
								item.Erase();
							}
						}
					}

					trans.Commit();
					doc.SendStringToExecute("._z e ", false, false, false);
				}

				catch (System.Exception ex)
				{
					doc.Editor.WriteMessage($"An error occured: {ex.Message}");
					trans.Abort();

				}
			}
		}

		[CommandMethod("MOVEX")]
		public static void MoveExercise()
		{
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			using (Transaction trans = doc.TransactionManager.StartTransaction())
			{
				try
				{
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					using (MText text1 = new MText())
					{
						text1.ColorIndex = 3;
						text1.Height = 5;
						text1.Contents = "Move Me";
						text1.Location = new Point3d(0, 0, 0);
                        btr.AppendEntity(text1);
                        trans.AddNewlyCreatedDBObject(text1, true);


                        MText text2 = new MText();
						text2.ColorIndex = 2;
                        text2.Height = 5;
                        text2.Contents = "Don't Move Me";
                        text2.Location = new Point3d(0, 0, 0);
                        btr.AppendEntity(text2);
                        trans.AddNewlyCreatedDBObject(text2, true);

                        MText text3 = new MText();
                        text3.ColorIndex = 1;
                        text3.Height = 5;
                        text3.Contents = "Don't Move Me Either";
                        text3.Location = new Point3d(0, 0, 0);
                        btr.AppendEntity(text3);
                        trans.AddNewlyCreatedDBObject(text3, true);

                        DBObjectCollection collection = new DBObjectCollection();
                        collection.Add(text1);
                        collection.Add(text2);
                        collection.Add(text3);

						foreach (MText text in collection)
						{
							if (text.Text == "Move Me")
							{
								Vector3d destinationVector = text.Location.GetVectorTo(new Point3d(50, 50, 0));
								
								text.TransformBy(Matrix3d.Displacement(destinationVector));	
							}
						}
                    }
                    trans.Commit();
                    doc.SendStringToExecute("._z e ", false, false, false);
                }
				catch (System.Exception ex)
				{
					doc.Editor.WriteMessage($"An error has occurred: {ex.Message}");
					trans.Abort();
				}

				
			}
		}

        [CommandMethod("MIRRORX")]
        public static void MirrorExercise()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt;
                    bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord btr;
                    btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					string textContent = "Mirrored";
                    using (MText text1 = new MText())
                    {
                        text1.ColorIndex = 1;
                        text1.Height = 3;
                        text1.Contents = textContent;
                        text1.Location = new Point3d(0, 0, 0);
                        btr.AppendEntity(text1);
                        trans.AddNewlyCreatedDBObject(text1, true);


                        MText text2 = new MText();
                        text2.ColorIndex = 2;
                        text2.Height = 5;
                        text2.Contents = textContent;
                        text2.Location = new Point3d(10, 0, 0);
                        btr.AppendEntity(text2);
                        trans.AddNewlyCreatedDBObject(text2, true);

                        MText text3 = new MText();
                        text3.ColorIndex = 2;
                        text3.Height = 5;
                        text3.Contents = textContent;
                        text3.Location = new Point3d(-10, 0, 0);
                        btr.AppendEntity(text3);
                        trans.AddNewlyCreatedDBObject(text3, true);
                        

                        DBObjectCollection collection = new DBObjectCollection();
                        collection.Add(text1);
                        collection.Add(text2);
                        collection.Add(text3);

                        foreach (MText text in collection)
                        {
                            if (text.Height == 3 && text.ColorIndex == 1)
                            {
                                Point3d ptFrom = new Point3d(0,15,0);
                                Point3d ptTo = new Point3d(20, 15, 0);
								Line3d mirrorLine = new Line3d(ptFrom, ptTo);

                                text.TransformBy(Matrix3d.Mirroring(mirrorLine));
                            }
                        }
                    }
                    trans.Commit();
                    doc.SendStringToExecute("._z e ", false, false, false);
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"An error has occurred: {ex.Message}");
                    trans.Abort();
                }


            }
        }

        [CommandMethod("ROTATEX")]
        public static void RotateExercise()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt;
                    bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord btr;
                    btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    using (MText text1 = new MText())
                    {
                        text1.ColorIndex = 0;
                        text1.Height = 5;
                        text1.Contents = "Rotating MText";
                        text1.Location = new Point3d(10, 10, 0);
                        btr.AppendEntity(text1);
                        trans.AddNewlyCreatedDBObject(text1, true);


                        MText text2 = new MText();
                        text2.ColorIndex = 1;
                        text2.Height = 5;
                        text2.Contents = "Rotated MText";
                        text2.Location = new Point3d(10, 10, 0);
                        btr.AppendEntity(text2);
                        trans.AddNewlyCreatedDBObject(text2, true);

						Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
						CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

						text2.TransformBy(Matrix3d.Rotation(0.523599, curUCS.Zaxis, new Point3d(0, 0, 0)));

                    }
                    trans.Commit();
                    doc.SendStringToExecute("._z e ", false, false, false);
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"An error has occurred: {ex.Message}");
                    trans.Abort();
                }


            }
        }

        [CommandMethod("SCALEX")]
        public static void ScaleExercise()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt;
                    bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord btr;
                    btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    using (Circle circle1 = new Circle())
                    {
                        circle1.Radius = 5;
                        circle1.Center = new Point3d(0, 0, 0);
                        btr.AppendEntity(circle1);
                        trans.AddNewlyCreatedDBObject(circle1, true);

                        Circle circle2 = new Circle();
                        circle2.Radius = 2.5;
                        circle2.Center = new Point3d(10, 0, 0);
                        circle2.ColorIndex = 1;
                        btr.AppendEntity(circle2);
                        trans.AddNewlyCreatedDBObject(circle2, true);

                        Circle circle3 = new Circle();
                        circle3.Radius = 5;
                        circle3.Center = new Point3d(20, 0, 0);
                        btr.AppendEntity(circle3);
                        trans.AddNewlyCreatedDBObject(circle3, true);
                        
                        DBObjectCollection collection = new DBObjectCollection();
                        collection.Add(circle1);
                        collection.Add(circle2);
                        collection.Add(circle3);

                        foreach (Circle circle in collection)
                        {
                            if (circle.Radius == 2.5 && circle.ColorIndex == 1)
                            {
                                circle.TransformBy(Matrix3d.Scaling(4, circle.Center));
                            }
                        }
                    }

                    trans.Commit();
                    doc.SendStringToExecute("._z e ", false, false, false);
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"An error has occurred: {ex.Message}");
                    trans.Abort();
                }


            }
        }
    }
}
