using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace DrawObjects
{
	public class DrawObject
	{
		[CommandMethod("DrawMText")]
		public void DrawMText()
		{
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;
			Editor edt = doc.Editor;

			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					edt.WriteMessage("Drawing MText Exercise!");

					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Specify the MTexts parameters (e.g. textString, insertionPoint)
					string txt = "Hello AutoCAD from C#!";
					Point3d insPt = new Point3d(200, 200, 0);

					//Creating an MText
					using (MText mtx = new MText())
					{
						mtx.Contents = txt;
						mtx.Location = insPt;

						//Appending it to the block table record
						btr.AppendEntity(mtx);
						trans.AddNewlyCreatedDBObject(mtx, true);
					}
						trans.Commit();	
				}
				catch (System.Exception ex)
				{
					edt.WriteMessage($"Error encountered: {ex.Message}");
					trans.Abort();

				}
			}
		}

		[CommandMethod("DrawCircle")]
		public void DrawCircle()
		{
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					doc.Editor.WriteMessage("Drawing a Circle!");

					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Specify the Circle parameters (i.e. centerPoint, radius, etc. etc.)
					Point3d centerPt = new Point3d(100, 100, 0);
					double circleRad = 100.0;

					//Creating a circle
					using (Circle circle = new Circle())
					{
						circle.Center = centerPt;
						circle.Radius = circleRad;

						//Appending it to the block table record
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

		[CommandMethod("DrawArc")]
		public void DrawArc()
		{
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;
			
			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					doc.Editor.WriteMessage("Drawing an Arc!");

					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Specify the arc parameters (e.g. centerPoint, radius, startAngle, endAngle)
					Point3d centerPt = new Point3d(10, 10, 0);
					double arcRad = 20.0;
					double startAngle = 1.0;
					double endAngle = 3.0;

					//Creating an Arc
					Arc arc = new Arc(centerPt, arcRad, startAngle, endAngle);

					//Set the default properties
					arc.SetDatabaseDefaults();

					btr.AppendEntity(arc);
					trans.AddNewlyCreatedDBObject(arc, true);
					trans.Commit();
				}
				catch (System.Exception ex)
				{
					doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
					trans.Abort();

				}
			}
		}

		[CommandMethod("DrawPLine")]
		public void DrawPLine()
		{
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;

			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					doc.Editor.WriteMessage("Drawing a polyline!");

					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Creating a polyline and specifying the Polyline's coordinates
					Polyline pl = new Polyline();
					pl.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
					pl.AddVertexAt(1, new Point2d(10, 10), 0, 0, 0);
					pl.AddVertexAt(2, new Point2d(20, 20), 0, 0, 0);
					pl.AddVertexAt(3, new Point2d(30, 30), 0, 0, 0);
					pl.AddVertexAt(4, new Point2d(40, 40), 0, 0, 0);
					pl.AddVertexAt(5, new Point2d(50, 50), 0, 0, 0);

					//Set the default properties
					pl.SetDatabaseDefaults();

					btr.AppendEntity(pl);
					trans.AddNewlyCreatedDBObject(pl, true);
					trans.Commit();
				}
				catch (System.Exception ex)
				{
					doc.Editor.WriteMessage($"Error encountered: {ex.Message}");
					trans.Abort();

				}
			}
		}

		[CommandMethod("DrawLine")]
		public void DrawLine()
		{
			Document doc = Application.DocumentManager.MdiActiveDocument;
			Database db = doc.Database;
			Editor edt = doc.Editor;

			using (Transaction trans = db.TransactionManager.StartTransaction())
			{
				try
				{
					BlockTable bt;
					bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

					BlockTableRecord btr;
					btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//Sending message to the user
					edt.WriteMessage("\nDrawing a Line object: ");

					//Creating a Line
					Point3d pt1 = new Point3d(0, 0, 0);
					Point3d pt2 = new Point3d(100, 100, 0);
					Line ln = new Line(pt1, pt2);
					ln.ColorIndex = 1; //Setting the color to red

					//Appending it to the block table record
					btr.AppendEntity(ln);
					trans.AddNewlyCreatedDBObject(ln, true);
					trans.Commit();

				}
				catch (System.Exception ex)
				{
					edt.WriteMessage($"Error encountered: {ex.Message}");
					trans.Abort();
				
				}
			}
		}
		
	}
}
