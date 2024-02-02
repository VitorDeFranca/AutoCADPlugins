using System.Drawing;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System.Windows;
using Autodesk.AutoCAD.Windows;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCadTraining
{
	public class MyCommands
    {
		//CONTROLLING THE APPLICATION WINDOW

		[CommandMethod("PositionApplicationWindow")]
		public static void PositionApplicationWindow()
		{
			// Set the position of the Application window
			Point ptApp = new Point(0, 0);
            Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.DeviceIndependentLocation = ptApp;

			// Set the size of the Application window
			Size szApp = new Size(400, 400);
            Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.DeviceIndependentSize = szApp;
		}

		[CommandMethod("MinMaxApplicationWindow")]
		public static void MinMaxApplicationWindow()
		{
			//Minimize the Application window
			Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.WindowState = Window.State.Minimized;


			System.Windows.Forms.MessageBox.Show("Minimized", "MinMax",
						System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.None,
						System.Windows.Forms.MessageBoxDefaultButton.Button1,
						System.Windows.Forms.MessageBoxOptions.ServiceNotification);

			//Maximize the Application window
			Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.WindowState = Window.State.Maximized;
			System.Windows.Forms.MessageBox.Show("Maximized", "MinMax");
		}

		[CommandMethod("CurrentWindowState")]
		public static void CurrentWindowState()
		{
			System.Windows.Forms.MessageBox.Show("The application window is " +
                                                    Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.WindowState.ToString(),
													"Window State");
		}

		[CommandMethod("HideWindowState")]
		public static void HideWindowState()
		{
            //Hide the Application window
            Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Visible = false;
			System.Windows.Forms.MessageBox.Show("Invisible", "Show/Hide");

			//Show the Application window
			Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Visible = true;
			System.Windows.Forms.MessageBox.Show("Visible", "Show/Hide");
		}


		//CONTROLLING THE DRAWING WINDOWS

		[CommandMethod("SizeDocumentWindow")]
		public static void SizeDocumentWindow()
		{
			//Size the Document window
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

			// Works around what looks to be a refresh problem with the Application window
			acDoc.Window.WindowState = Window.State.Normal;

			// Set the position of the Document window
			System.Windows.Point ptDoc = new System.Windows.Point(0, 0);
			acDoc.Window.DeviceIndependentLocation = ptDoc;

			// Set the size of the Document window
			System.Windows.Size szDoc = new System.Windows.Size(400, 400);
			acDoc.Window.DeviceIndependentSize = szDoc;
		}

		[CommandMethod("MinMaxDocumentWindow")]
		public static void MinMaxDocumentWindow()
		{
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

			//Minimize the Document window
			acDoc.Window.WindowState = Window.State.Minimized;
			System.Windows.Forms.MessageBox.Show("Minimized", "MinMax");

			//Maximize the Document window
			acDoc.Window.WindowState = Window.State.Maximized;
			System.Windows.Forms.MessageBox.Show("Maximized", "MinMax");
		}

		[CommandMethod("CurrentDocWindowState")]
		public static void CurrentDocWindowState()
		{
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

			System.Windows.Forms.MessageBox.Show("The document window is " +
			acDoc.Window.WindowState.ToString(), "Window State");
		}


		//MANIPULATING THE CURRENT VIEW

		[CommandMethod("ZoomWindow")]
		static public void ZoomWindow()
		{
			// Zoom to a window boundary defined by 1.3,7.8 and 13.7,-2.6
			Point3d pMin = new Point3d(1.3, 7.8, 0);
			Point3d pMax = new Point3d(13.7, -2.6, 0);

			Zoom(pMin, pMax, new Point3d(), 1);
		}

		[CommandMethod("ZoomScale")]
		static public void ZoomScale()
		{
			// Get the current document
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

			// Get the current view
			using (ViewTableRecord acView = acDoc.Editor.GetCurrentView())
			{
				// Get the center of the current view
				Point3d pCenter = new Point3d(acView.CenterPoint.X,
											  acView.CenterPoint.Y, 0);

				// Set the scale factor to use
				double dScale = 0.5;

				// Scale the view using the center of the current view
				Zoom(new Point3d(), new Point3d(), pCenter, 1 / dScale);
			}
		}

		[CommandMethod("ZoomCenter")]
		static public void ZoomCenter()
		{
			// Center the view at 5,5,0
			Zoom(new Point3d(), new Point3d(), new Point3d(5, 5, 0), 1);
		}

		[CommandMethod("ZoomExtents")]
		static public void ZoomExtents()
		{
			// Zoom to the extents of the current space
			Zoom(new Point3d(), new Point3d(), new Point3d(), 1.01075);
		}

		[CommandMethod("ZoomLimits")]
		static public void ZoomLimits()
		{
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database acCurDb = acDoc.Database;

			// Zoom to the limits of Model space
			Zoom(new Point3d(acCurDb.Limmin.X, acCurDb.Limmin.Y, 0),
				 new Point3d(acCurDb.Limmax.X, acCurDb.Limmax.Y, 0),
				 new Point3d(), 1);
		}


		//CREATING AND NAMING A VIEW

		[CommandMethod("CreateNamedView")]
		public static void CreateNamedView()
		{
			// Get the current database
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database acCurDb = acDoc.Database;

			// Start a transaction
			using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
			{
				// Open the View table for read
				ViewTable acViewTbl;
				acViewTbl = acTrans.GetObject(acCurDb.ViewTableId,
												OpenMode.ForRead) as ViewTable;

				// Check to see if the named view 'View1' exists
				if (acViewTbl.Has("View1") == false)
				{
					// Open the View table for write
					acTrans.GetObject(acCurDb.ViewTableId, OpenMode.ForWrite);

					// Create a new View table record and name the view 'View1'
					using (ViewTableRecord acViewTblRec = new ViewTableRecord())
					{
						acViewTblRec.Name = "View1";

						// Add the new View table record to the View table and the transaction
						acViewTbl.Add(acViewTblRec);
						acTrans.AddNewlyCreatedDBObject(acViewTblRec, true);

						// Set 'View1' current
						acDoc.Editor.SetCurrentView(acViewTblRec);
					}

					// Commit the changes
					acTrans.Commit();
				}

				// Dispose of the transaction
			}
		}

		[CommandMethod("EraseNamedView")]
		public static void EraseNamedView()
		{
			// Get the current database
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database acCurDb = acDoc.Database;

			// Start a transaction
			using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
			{
				// Open the View table for read
				ViewTable acViewTbl;
				acViewTbl = acTrans.GetObject(acCurDb.ViewTableId,
												OpenMode.ForRead) as ViewTable;

				// Check to see if the named view 'View1' exists
				if (acViewTbl.Has("View1") == true)
				{
					// Open the View table for write
					acTrans.GetObject(acCurDb.ViewTableId, OpenMode.ForWrite);

					// Get the named view
					ViewTableRecord acViewTblRec;
					acViewTblRec = acTrans.GetObject(acViewTbl["View1"],
														OpenMode.ForWrite) as ViewTableRecord;

					// Remove the named view from the View table
					acViewTblRec.Erase();

					// Commit the changes
					acTrans.Commit();
				}

				// Dispose of the transaction
			}
		}

		[CommandMethod("CreateModelViewport")]
		public static void CreateModelViewport()
		{
			// Get the current database
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database acCurDb = acDoc.Database;

			// Start a transaction
			using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
			{
				// Open the Viewport table for read
				ViewportTable acVportTbl;
				acVportTbl = acTrans.GetObject(acCurDb.ViewportTableId,
												OpenMode.ForRead) as ViewportTable;

				// Check to see if the named view 'TEST_VIEWPORT' exists
				if (acVportTbl.Has("TEST_VIEWPORT") == false)
				{
					// Open the View table for write
					acTrans.GetObject(acCurDb.ViewportTableId, OpenMode.ForWrite);

					// Add the new viewport to the Viewport table and the transaction
					using (ViewportTableRecord acVportTblRecLwr = new ViewportTableRecord())
					{
						acVportTbl.Add(acVportTblRecLwr);
						acTrans.AddNewlyCreatedDBObject(acVportTblRecLwr, true);

						// Name the new viewport 'TEST_VIEWPORT' and assign it to be
						// the lower half of the drawing window
						acVportTblRecLwr.Name = "TEST_VIEWPORT";
						acVportTblRecLwr.LowerLeftCorner = new Point2d(0, 0);
						acVportTblRecLwr.UpperRightCorner = new Point2d(1, 0.5);

						// Add the new viewport to the Viewport table and the transaction
						using (ViewportTableRecord acVportTblRecUpr = new ViewportTableRecord())
						{
							acVportTbl.Add(acVportTblRecUpr);
							acTrans.AddNewlyCreatedDBObject(acVportTblRecUpr, true);

							// Name the new viewport 'TEST_VIEWPORT' and assign it to be
							// the upper half of the drawing window
							acVportTblRecUpr.Name = "TEST_VIEWPORT";
							acVportTblRecUpr.LowerLeftCorner = new Point2d(0, 0.5);
							acVportTblRecUpr.UpperRightCorner = new Point2d(1, 1);

							// To assign the new viewports as the active viewports, the 
							// viewports named '*Active' need to be removed and recreated
							// based on 'TEST_VIEWPORT'.

							// Step through each object in the symbol table
							foreach (ObjectId acObjId in acVportTbl)
							{
								// Open the object for read
								ViewportTableRecord acVportTblRec;
								acVportTblRec = acTrans.GetObject(acObjId,
																	OpenMode.ForRead) as ViewportTableRecord;

								// See if it is one of the active viewports, and if so erase it
								if (acVportTblRec.Name == "*Active")
								{
									acTrans.GetObject(acObjId, OpenMode.ForWrite);
									acVportTblRec.Erase();
								}
							}

							// Clone the new viewports as the active viewports
							foreach (ObjectId acObjId in acVportTbl)
							{
								// Open the object for read
								ViewportTableRecord acVportTblRec;
								acVportTblRec = acTrans.GetObject(acObjId,
																	OpenMode.ForRead) as ViewportTableRecord;

								// See if it is one of the viewports that you want to activate and clone them as active
								if (acVportTblRec.Name == "TEST_VIEWPORT")
								{
									ViewportTableRecord acVportTblRecClone;
									acVportTblRecClone = acVportTblRec.Clone() as ViewportTableRecord;

									// Add the new viewport to the Viewport table and the transaction
									acVportTbl.Add(acVportTblRecClone);
									acVportTblRecClone.Name = "*Active";
									acTrans.AddNewlyCreatedDBObject(acVportTblRecClone, true);
								}
							}

							// Update the display with the new tiled viewports arrangement
							acDoc.Editor.UpdateTiledViewportsFromDatabase();
						}
					}

					// Commit the changes
					acTrans.Commit();
				}

				// Dispose of the transaction
			}
		}

		[CommandMethod("SplitAndIterateModelViewports")]
		public static void SplitAndIterateModelViewports()
		{
			// Get the current database
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database acCurDb = acDoc.Database;

			// Start a transaction
			using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
			{
				// Open the Viewport table for write
				ViewportTable acVportTbl;
				acVportTbl = acTrans.GetObject(acCurDb.ViewportTableId,
												OpenMode.ForWrite) as ViewportTable;

				// Open the active viewport for write
				ViewportTableRecord acVportTblRec;
				acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
													OpenMode.ForWrite) as ViewportTableRecord;

				using (ViewportTableRecord acVportTblRecNew = new ViewportTableRecord())
				{
					// Add the new viewport to the Viewport table and the transaction
					acVportTbl.Add(acVportTblRecNew);
					acTrans.AddNewlyCreatedDBObject(acVportTblRecNew, true);

					// Assign the name '*Active' to the new Viewport
					acVportTblRecNew.Name = "*Active";

					// Use the existing lower left corner for the new viewport
					acVportTblRecNew.LowerLeftCorner = acVportTblRec.LowerLeftCorner;

					// Get half the X of the existing upper corner
					acVportTblRecNew.UpperRightCorner = new Point2d(acVportTblRec.UpperRightCorner.X,
																	acVportTblRec.LowerLeftCorner.Y +
																	((acVportTblRec.UpperRightCorner.Y -
																		acVportTblRec.LowerLeftCorner.Y) / 2));

					// Recalculate the corner of the active viewport
					acVportTblRec.LowerLeftCorner = new Point2d(acVportTblRec.LowerLeftCorner.X,
																acVportTblRecNew.UpperRightCorner.Y);

					// Update the display with the new tiled viewports arrangement
					acDoc.Editor.UpdateTiledViewportsFromDatabase();

					// Step through each object in the symbol table
					foreach (ObjectId acObjId in acVportTbl)
					{
						// Open the object for read
						ViewportTableRecord acVportTblRecCur;
						acVportTblRecCur = acTrans.GetObject(acObjId,
																OpenMode.ForRead) as ViewportTableRecord;

						if (acVportTblRecCur.Name == "*Active")
						{
                            Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("CVPORT", acVportTblRecCur.Number);

							Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Viewport: " + acVportTblRecCur.Number +
														" is now active." +
														"\nLower left corner: " +
														acVportTblRecCur.LowerLeftCorner.X + ", " +
														acVportTblRecCur.LowerLeftCorner.Y +
														"\nUpper right corner: " +
														acVportTblRecCur.UpperRightCorner.X + ", " +
														acVportTblRecCur.UpperRightCorner.Y);
						}
					}
				}

				// Commit the changes and dispose of the transaction
				acTrans.Commit();
			}
		}

		//CREATING THE ZOOM FUNCTION
		static void Zoom(Point3d pMin, Point3d pMax, Point3d pCenter, double dFactor)
		{
			// Get the current document and database
			Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database acCurDb = acDoc.Database;

			int nCurVport = System.Convert.ToInt32(Autodesk.AutoCAD.ApplicationServices.Application.GetSystemVariable("CVPORT"));

			// Get the extents of the current space when no points 
			// or only a center point is provided
			// Check to see if Model space is current
			if (acCurDb.TileMode == true)
			{
				if (pMin.Equals(new Point3d()) == true &&
					pMax.Equals(new Point3d()) == true)
				{
					pMin = acCurDb.Extmin;
					pMax = acCurDb.Extmax;
				}
			}
			else
			{
				// Check to see if Paper space is current
				if (nCurVport == 1)
				{
					// Get the extents of Paper space
					if (pMin.Equals(new Point3d()) == true &&
						pMax.Equals(new Point3d()) == true)
					{
						pMin = acCurDb.Pextmin;
						pMax = acCurDb.Pextmax;
					}
				}
				else
				{
					// Get the extents of Model space
					if (pMin.Equals(new Point3d()) == true &&
						pMax.Equals(new Point3d()) == true)
					{
						pMin = acCurDb.Extmin;
						pMax = acCurDb.Extmax;
					}
				}
			}

			// Start a transaction
			using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
			{
				// Get the current view
				using (ViewTableRecord acView = acDoc.Editor.GetCurrentView())
				{
					Extents3d eExtents;

					// Translate WCS coordinates to DCS
					Matrix3d matWCS2DCS;
					matWCS2DCS = Matrix3d.PlaneToWorld(acView.ViewDirection);
					matWCS2DCS = Matrix3d.Displacement(acView.Target - Point3d.Origin) * matWCS2DCS;
					matWCS2DCS = Matrix3d.Rotation(-acView.ViewTwist,
													acView.ViewDirection,
													acView.Target) * matWCS2DCS;

					// If a center point is specified, define the min and max 
					// point of the extents
					// for Center and Scale modes
					if (pCenter.DistanceTo(Point3d.Origin) != 0)
					{
						pMin = new Point3d(pCenter.X - (acView.Width / 2),
											pCenter.Y - (acView.Height / 2), 0);

						pMax = new Point3d((acView.Width / 2) + pCenter.X,
											(acView.Height / 2) + pCenter.Y, 0);
					}

					// Create an extents object using a line
					using (Line acLine = new Line(pMin, pMax))
					{
						eExtents = new Extents3d(acLine.Bounds.Value.MinPoint,
													acLine.Bounds.Value.MaxPoint);
					}

					// Calculate the ratio between the width and height of the current view
					double dViewRatio;
					dViewRatio = (acView.Width / acView.Height);

					// Tranform the extents of the view
					matWCS2DCS = matWCS2DCS.Inverse();
					eExtents.TransformBy(matWCS2DCS);

					double dWidth;
					double dHeight;
					Point2d pNewCentPt;

					// Check to see if a center point was provided (Center and Scale modes)
					if (pCenter.DistanceTo(Point3d.Origin) != 0)
					{
						dWidth = acView.Width;
						dHeight = acView.Height;

						if (dFactor == 0)
						{
							pCenter = pCenter.TransformBy(matWCS2DCS);
						}

						pNewCentPt = new Point2d(pCenter.X, pCenter.Y);
					}
					else // Working in Window, Extents and Limits mode
					{
						// Calculate the new width and height of the current view
						dWidth = eExtents.MaxPoint.X - eExtents.MinPoint.X;
						dHeight = eExtents.MaxPoint.Y - eExtents.MinPoint.Y;

						// Get the center of the view
						pNewCentPt = new Point2d(((eExtents.MaxPoint.X + eExtents.MinPoint.X) * 0.5),
													((eExtents.MaxPoint.Y + eExtents.MinPoint.Y) * 0.5));
					}

					// Check to see if the new width fits in current window
					if (dWidth > (dHeight * dViewRatio)) dHeight = dWidth / dViewRatio;

					// Resize and scale the view
					if (dFactor != 0)
					{
						acView.Height = dHeight * dFactor;
						acView.Width = dWidth * dFactor;
					}

					// Set the center of the view
					acView.CenterPoint = pNewCentPt;

					// Set the current view
					acDoc.Editor.SetCurrentView(acView);
				}

				// Commit the changes
				acTrans.Commit();
			}
		}
	}


}
