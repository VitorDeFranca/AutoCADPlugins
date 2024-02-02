using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;


namespace ManipulateObjects
{
	public class ManipulateObject
	{
		[CommandMethod("SINGLECOPY")]
		public static void SingleCopy()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Create a circle that is 2,3 with a radius of 4.25
					using (Circle c1 = new Circle())
					{
						c1.Center = new Point3d(2, 3, 0);
						c1.Radius = 4.25;

						//Add the new object to the Blocktable record
						btr.AppendEntity(c1);
						trans.AddNewlyCreatedDBObject(c1, true);

						//Create a copy of the circle and change its radius
						Circle c1Clone = c1.Clone() as Circle;
						c1Clone.Radius = 1;

						//Add the cloned circle
						btr.AppendEntity(c1Clone);
						trans.AddNewlyCreatedDBObject(c1Clone, true);
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

		[CommandMethod("MULTIPLECOPY")]
		public static void MultipleCopy()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					using (Circle c1 = new Circle())
					{
						c1.Center = new Point3d(0, 0, 0);
						c1.Radius = 5;

						btr.AppendEntity(c1);
						trans.AddNewlyCreatedDBObject(c1, true);

						using (Circle c2 = new Circle())
						{
							c2.Center = new Point3d(0, 0, 0);
							c2.Radius = 7;

							btr.AppendEntity(c2);
							trans.AddNewlyCreatedDBObject(c2, true);

							DBObjectCollection col = new DBObjectCollection();
							col.Add(c1);
							col.Add(c2);

							foreach (Entity acEnt in col)
							{
								Entity ent;
								ent = acEnt.Clone() as Entity;
								ent.ColorIndex = 1;

								//Create matrix and move each copied entity 20 units to the right
								ent.TransformBy(Matrix3d.Displacement(new Vector3d(20, 0, 0)));

								//Add the cloned object to the BlockTable record
								btr.AppendEntity(ent);
								trans.AddNewlyCreatedDBObject(ent, true);

							}

							trans.Commit();

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

		[CommandMethod("ERASEOBJECT")]
		public static void EraseObject()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Create a lightweight polyline 
					using (Polyline pl = new Polyline())
					{
						pl.AddVertexAt(0, new Point2d(2, 4), 0, 0, 0);
						pl.AddVertexAt(1, new Point2d(4, 2), 0, 0, 0);
						pl.AddVertexAt(2, new Point2d(6, 4), 0, 0, 0);

						//Add the new object to the block table record
						btr.AppendEntity(pl);
						trans.AddNewlyCreatedDBObject(pl, true);

						//Execute an autocad command
						doc.SendStringToExecute("._z e ", false, false, false);


						//Update th display and display an alert message
						doc.Editor.Regen();
						Application.ShowAlertDialog("Erasing the newly created polyline.");

						pl.Erase(true);

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

		[CommandMethod("MOVEOBJECT")]
		public static void MoveObject()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Create a circle that it's center is at 2,2 and it's radius is 0.5
					using (Circle circle = new Circle())
					{
						circle.Center = new Point3d(2, 2, 2);
						circle.Radius = 0.5;

						//Create a matrix and move the circle using a vector from (0,0,0) to (2,0,0)
						Point3d startPoint = new Point3d(0, 0, 0);
						Vector3d destinationVector = startPoint.GetVectorTo(new Point3d(2, 0, 0));

						circle.TransformBy(Matrix3d.Displacement(destinationVector));

						btr.AppendEntity(circle);
						trans.AddNewlyCreatedDBObject(circle, true);

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

		[CommandMethod("MIRROROBJECT")]
		public static void MirrorObject()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Create a lightweight polyline
					using (Polyline pl = new Polyline())
					{
						pl.AddVertexAt(0, new Point2d(1, 1), 0, 0, 0);
						pl.AddVertexAt(1, new Point2d(1, 2), 0, 0, 0);
						pl.AddVertexAt(2, new Point2d(2, 2), 0, 0, 0);
						pl.AddVertexAt(3, new Point2d(3, 2), 0, 0, 0);
						pl.AddVertexAt(4, new Point2d(4, 4), 0, 0, 0);
						pl.AddVertexAt(5, new Point2d(4, 1), 0, 0, 0);

						//Create a bulge of -2 at vertex 1
						pl.SetBulgeAt(1, -2);

						//Close the polyline
						pl.Closed = true;

						btr.AppendEntity(pl);
						trans.AddNewlyCreatedDBObject(pl, true);

						//Create the polyline to be mirrored
						Polyline plMirror = pl.Clone() as Polyline;
						plMirror.ColorIndex = 5;

						//Define the mirror line
						Point3d ptFrom = new Point3d(0, 4.25, 0);
						Point3d ptTo = new Point3d(4, 4.25, 0);
						Line3d line = new Line3d(ptFrom, ptTo);

						//Mirror the polyline across de X axis
						plMirror.TransformBy(Matrix3d.Mirroring(line));

						btr.AppendEntity(plMirror);
						trans.AddNewlyCreatedDBObject(plMirror, true);


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

		[CommandMethod("ROTATEOBJECT")]
		public static void RotateObject()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Create a new polyline
					using (Polyline pl = new Polyline())
					{
						pl.AddVertexAt(0, new Point2d(1, 2), 0, 0, 0);
						pl.AddVertexAt(1, new Point2d(1, 3), 0, 0, 0);
						pl.AddVertexAt(2, new Point2d(2, 3), 0, 0, 0);
						pl.AddVertexAt(3, new Point2d(3, 3), 0, 0, 0);
						pl.AddVertexAt(4, new Point2d(4, 4), 0, 0, 0);
						pl.AddVertexAt(5, new Point2d(4, 2), 0, 0, 0);

						pl.Closed = true;

						Matrix3d curUCSMAtrix = doc.Editor.CurrentUserCoordinateSystem;
						CoordinateSystem3d curUCS = curUCSMAtrix.CoordinateSystem3d;

						//Rotate the polyline 45 degrees, around the Z-axis of the current UCS
						//Using a base point of (4, 4.25, 0)
						pl.TransformBy(Matrix3d.Rotation(0.7854, curUCS.Zaxis, new Point3d(4, 4.25, 0)));

						btr.AppendEntity(pl);
						trans.AddNewlyCreatedDBObject(pl, true);

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

		[CommandMethod("SCALEOBJECT")]
		public static void ScaleObject()
		{
			//Get the current document and database
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			//Start a transaction
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					//Open the BlockTable for reading
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Open the BlockTable record ModelSpace for writing
					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Create a new polyline
					using (Polyline pl = new Polyline())
					{
						pl.AddVertexAt(0, new Point2d(1, 2), 0, 0, 0);
						pl.AddVertexAt(1, new Point2d(1, 3), 0, 0, 0);
						pl.AddVertexAt(2, new Point2d(2, 3), 0, 0, 0);
						pl.AddVertexAt(3, new Point2d(3, 3), 0, 0, 0);
						pl.AddVertexAt(4, new Point2d(4, 4), 0, 0, 0);
						pl.AddVertexAt(5, new Point2d(4, 2), 0, 0, 0);

						pl.Closed = true;

						//Reduce the object by a factor of 0.5 using a base point of (4, 4.25, 0)
						pl.TransformBy(Matrix3d.Scaling(0.5, new Point3d(4, 4.25, 0)));

						btr.AppendEntity(pl);
						trans.AddNewlyCreatedDBObject(pl, true);

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
	}
}
