using Mypaint.View;
using MyPaint.Shape;
using MyPaint.CurentShape;
using MyPaint.Find;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyPaint.DRAW
{
    class DrawShape
    {
       
        ViewPaint viewPaint;
        DataManager dataManager;

        public DrawShape(ViewPaint viewPaint)
        {
            this.viewPaint = viewPaint;
            dataManager = DataManager.getInstance();
        }

        public void onClickMouseDown(Point p)
        {
            dataManager.isSave = false;
            dataManager.isNotNone = true;
            if (dataManager.currentShape.Equals(CurrentShape.Cursor))
            {
                if (!(Control.ModifierKeys == Keys.Control))
                    dataManager.offAllShapeSelected();
                viewPaint.refreshDrawing();
                handleClickToSelect(p);
            }
            else
            {
                handleClickToDraw(p);
            }
        }

        public void handleClickToSelect(Point p)
        {
            for (int i = 0; i < dataManager.shapeList.Count; ++i)
            {
                if (!(dataManager.shapeList[i] is MPen))
                    dataManager.pointToResize = dataManager.shapeList[i].GetpointResize(p);
                if (dataManager.pointToResize != -1)
                {
                    dataManager.shapeList[i].changePoint(dataManager.pointToResize);
                    dataManager.shapeToMove = dataManager.shapeList[i];
                    break;
                }
                else if (isInsidePoint(dataManager.shapeList[i], p))
                {
                    dataManager.shapeToMove = dataManager.shapeList[i];
                    dataManager.shapeList[i].isSelected = true;                 
                    break;
                }
            }

            if (dataManager.pointToResize != -1)
            {
                dataManager.cursorCurrent = p;
            }
            else if (dataManager.shapeToMove != null)
            {
                dataManager.isMovingShape = true;
                dataManager.cursorCurrent = p;
            }
            else
            {
                dataManager.isMovingMouse = true;
                dataManager.rectangleRegion = new Rectangle(p, new Size(0, 0));
            }
        }

        private void handleClickToDraw(Point p)
        {
            dataManager.isMouseDown = true;
            dataManager.offAllShapeSelected();
            if (dataManager.currentShape.Equals(CurrentShape.Rectangle))
            {
                dataManager.addShape(new MRectangle
                {
                    pointHead = p,
                    pointTail = p,
                    pen = dataManager.Pen,
                    color = dataManager.colorCurrent,
                    isFill = dataManager.isFill
                }) ;
            }
            if (dataManager.currentShape.Equals(CurrentShape.Ellipse))
            {
                dataManager.addShape(new MEllipse
                {
                    pointHead = p,
                    pointTail = p,
                    pen = dataManager.Pen,
                    color = dataManager.colorCurrent,
                    isFill = dataManager.isFill
                });
            }
            if (dataManager.currentShape.Equals(CurrentShape.Line))
            {
                dataManager.addShape(new MLine
                {
                    pointHead = p,
                    pointTail = p,
                    pen = dataManager.Pen,
                    color = dataManager.colorCurrent,
                    isFill = dataManager.isFill
                });
            }

            else if (dataManager.currentShape.Equals(CurrentShape.Polygon))
            {
                if (!dataManager.isDrawingPolygon)
                {
                    dataManager.isDrawingPolygon = true;
                    MPolygon polygon = new MPolygon
                    {
                        color = dataManager.colorCurrent,
                        pen = dataManager.Pen,
                        isFill = dataManager.isFill

                    };
                    polygon.points.Add(p);
                    polygon.points.Add(p);
                    dataManager.addShape(polygon);
                }
                else
                {
                    MPolygon polygon = dataManager.shapeList[dataManager.shapeList.Count - 1] as MPolygon;
                    polygon.points[polygon.points.Count - 1] = p;
                    polygon.points.Add(p);
                }
                dataManager.isMouseDown = false;
            }

            else if (dataManager.currentShape.Equals(CurrentShape.Curve))
            {
                if (!dataManager.isDrawingCurve)
                {
                    dataManager.isDrawingCurve = true;
                    MCurve curve = new MCurve
                    {
                        color = dataManager.colorCurrent,
                        pen = dataManager.Pen,
                        isFill = dataManager.isFill
                    };
                    curve.points.Add(p);
                    curve.points.Add(p);
                    dataManager.addShape(curve );
                }
                else
                {
                    MCurve curve = dataManager.shapeList[dataManager.shapeList.Count - 1] as MCurve;
                    curve.points[curve.points.Count - 1] = p;
                    curve.points.Add(p);
                }
                dataManager.isMouseDown = false;
            }
            else if (dataManager.currentShape.Equals(CurrentShape.Pen))
            {
                dataManager.isDrawingPen = true;
                MPen pen = new MPen
                {
                    color = dataManager.colorCurrent,
                    pen = dataManager.Pen,
                    isFill = dataManager.isFill
                };
                pen.points.Add(p);
                pen.points.Add(p);
                dataManager.shapeList.Add(pen);
            }
            
        }

        public void onClickMouseMove(Point p)
        {
            if (dataManager.isMouseDown)
            {
                dataManager.updatePointTail(p);
                viewPaint.refreshDrawing();
            }
            else if (dataManager.pointToResize != -1)
            {
                if (!(dataManager.shapeToMove is MPen))
                {
                    moveControlPoint(dataManager.shapeToMove, p, dataManager.cursorCurrent, dataManager.pointToResize);
                    viewPaint.refreshDrawing();
                    dataManager.cursorCurrent = p;
                }
            }
            else if (dataManager.isMovingShape)
            {
                updateMovingShape(dataManager.distanceXY(dataManager.cursorCurrent, p), dataManager.shapeToMove);
                viewPaint.refreshDrawing();
                dataManager.cursorCurrent = p;
            }
            if (dataManager.isDrawingPolygon)
            {
                MPolygon polygon = dataManager.shapeList[dataManager.shapeList.Count - 1] as MPolygon;
                polygon.points[polygon.points.Count - 1] = p;
                viewPaint.refreshDrawing();
            }
            else if (dataManager.isDrawingCurve)
            {
                MCurve curve = dataManager.shapeList[dataManager.shapeList.Count - 1] as MCurve;
                curve.points[curve.points.Count - 1] = p;
                viewPaint.refreshDrawing();
            }
            else if (dataManager.isDrawingPen)
            {
                MPen pen = dataManager.shapeList[dataManager.shapeList.Count - 1] as MPen;
                pen.points.Add(p);
                FindPoints.setPointHeadTail(pen);
                viewPaint.refreshDrawing();
            }
            else if (dataManager.isDrawingEraser)
            {
                MPen pen = dataManager.shapeList[dataManager.shapeList.Count - 1] as MPen;
                pen.points.Add(p);
                FindPoints.setPointHeadTail(pen);
                viewPaint.refreshDrawing();
            }
            else if (dataManager.currentShape.Equals(CurrentShape.Cursor))
            {
                if (dataManager.isMovingMouse)
                {
                    dataManager.updateRectangleRegion(p);
                    viewPaint.refreshDrawing();
                }
                else
                {
                    if (dataManager.shapeList.Exists(shape => isInsidePoint(shape, p)))
                    {
                        viewPaint.setCursor(Cursors.SizeAll);
                    }
                    else
                    {
                        viewPaint.setCursor(Cursors.Default);
                    }
                }
            }
        }

        public void eventKeyDown(Panel Drawing, Keys key)
        {
            if (key == Keys.A && Control.ModifierKeys.HasFlag(Keys.Control))
            {
                for (int i = 0; i < dataManager.shapeList.Count; ++i)
                    dataManager.shapeList[i].isSelected = true;
                viewPaint.refreshDrawing();
            }
            if (key == Keys.Delete)
            {
                deleteShape();
            }
        }

        private bool isInsidePoint(SHAPE shape, System.Drawing.Point p)
        {
            bool inside = false;
            using (GraphicsPath path = shape.graphic)
            {
                inside = path.IsVisible(p);
            }
            if (shape is MLine)
            {
                using (GraphicsPath path = shape.graphic)
                {
                    using (Pen pen = new Pen(shape.color , shape.pen.Width + 3))
                    {
                        inside = path.IsOutlineVisible(p, pen);
                    }
                }
                return inside;
            }
            if (shape.groupnumber != -1)
                inside = false;
            return inside;
        }

        private bool isInsideShape(SHAPE shape, Rectangle rect)
        {
            if (shape.groupnumber != -1)
                return false;
            if (shape.pointHead.X >= rect.X &&
                shape.pointTail.X <= rect.X + rect.Width &&
                shape.pointHead.Y >= rect.Y &&
                shape.pointTail.Y <= rect.Y + rect.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void deleteShape()
        {
            int cnt = 0;
            List<Group> groupsCopy = new List<Group>(dataManager.groups);
            List<SHAPE> shapeListCopy = new List<SHAPE>(dataManager.shapeList);
            foreach (SHAPE shape in shapeListCopy)
            {
                if (shape.isSelected == true && isGrRetagle(shape)!=-1)
                {
                    int j = isGrRetagle(shape);
                    foreach (SHAPE shape2 in groupsCopy[j].shapeList)
                    {
                        foreach (SHAPE shape3 in shapeListCopy)
                        {
                            if (shape2 == shape3)
                            {
                                dataManager.shapeList.Remove(shape3);
                            }
                        }
                    }
                    dataManager.groups.Remove(groupsCopy[j]);
                    viewPaint.refreshDrawing();
                    cnt += 1;
                }
                if (shape.isSelected == true)
                {
                    dataManager.shapeList.Remove(shape);
                    
                    viewPaint.refreshDrawing();
                    cnt += 1;
                }
            }
            if (cnt == 0) { MessageBox.Show("Bạn cần chọn hình cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        public void fillShape()
        {
            foreach (SHAPE shape in dataManager.shapeList)
            {
                if (shape.isSelected == true)
                    if (shape.name == "retangle" || shape.name == "ellipse" || shape.name == "Polygon")
                        if (shape.isFill == false)
                        {
                            shape.isFill = true;
                            viewPaint.refreshDrawing();
                        }
            }
        }

        public void onClickSelect()
        {
            dataManager.currentShape = CurrentShape.Cursor;
        }

        public void onClickDrawRectangle()
        {
            setDefaultToDraw();
            dataManager.currentShape = CurrentShape.Rectangle;
        }

        public void onClickDrawFillRectangle()
        {
            setDefaultToDraw();
            dataManager.isFill = true;
            dataManager.currentShape = CurrentShape.Rectangle;
        }

        public void onClickDrawEllipse()
        {
            setDefaultToDraw();
            dataManager.currentShape = CurrentShape.Ellipse;
        }

        public void onClickDrawFillEllipse()
        {
            setDefaultToDraw();
            dataManager.isFill = true;  
            dataManager.currentShape = CurrentShape.Ellipse;
        }

        public void onClickDrawLine()
        {
            setDefaultToDraw();
            dataManager.currentShape = CurrentShape.Line;
        }

        public void onClickDrawPolygon()
        {
            setDefaultToDraw();
            dataManager.currentShape = CurrentShape.Polygon;
        }

        public void onClickDrawFillPolygon()
        {
            setDefaultToDraw();
            dataManager.isFill = true;
            dataManager.currentShape = CurrentShape.Polygon;
        }

        public void onClickDrawCurve()
        {
            setDefaultToDraw();
            dataManager.currentShape = CurrentShape.Curve;
        }   

        public void onClickDrawPen()
        {
            setDefaultToDraw();
            dataManager.currentShape = CurrentShape.Pen;
        }

        private void setDefaultToDraw()
        {
            dataManager.isFill = false;
            dataManager.offAllShapeSelected();
            viewPaint.refreshDrawing();
            viewPaint.setCursor(Cursors.Default);
        }

        public void getDrawing(Graphics g)
        {
            dataManager.shapeList.ForEach(shape =>
            {
                viewPaint.setDrawing(shape, g);
                if (shape.isSelected)
                {
                    drawRegionForShape(shape, g);
                }

            });
            if (dataManager.isMovingMouse)
            {
                using (Pen pen = new Pen(Color.DarkBlue, 1)
                {
                    DashPattern = new float[] { 3, 3, 3, 3 },
                    DashStyle = DashStyle.Custom
                })
                {
                    viewPaint.setDrawingRegionRectangle(pen, dataManager.rectangleRegion, g);
                }

            }
            if (dataManager.pointToResize != -1)
            {
                drawRegionForShape(dataManager.shapeToMove, g);
            }

        }

        public void onClickMouseUp()
        {
            dataManager.isMouseDown = false;
            if (dataManager.pointToResize != -1)
            {
                dataManager.pointToResize = -1;
                dataManager.shapeToMove = null;
            }
            else if (dataManager.isMovingShape)
            {
                dataManager.isMovingShape = false;
                dataManager.shapeToMove = null;
            }
            if (dataManager.isDrawingPen)
            {
                dataManager.isDrawingPen = false;
            }
            else if (dataManager.isDrawingEraser)
            {
                dataManager.isDrawingEraser = false;
            }
            else if (dataManager.isMovingMouse)
            {
                dataManager.isMovingMouse = false;
                dataManager.offAllShapeSelected();
                for (int i = 0; i < dataManager.shapeList.Count; ++i)
                {
                    if (isInsideShape(dataManager.shapeList[i], dataManager.rectangleRegion))
                    {
                        dataManager.shapeList[i].isSelected = true;
                    }
                    if (dataManager.shapeList[i] is MPen)
                    {
                        MPen pen = dataManager.shapeList[i] as MPen;                       
                    }
                    viewPaint.refreshDrawing();
                }
            }
        }

        private void drawRegionForShape(SHAPE shape, Graphics g)
        {
            if (shape is MLine)
            {
                viewPaint.setDrawingLineSelected(shape, new SolidBrush(Color.DarkBlue), g);

            }
            else if (shape is MPen)
            {
                using (Pen pen = new Pen(Color.Blue, 1)
                {
                    DashPattern = new float[] { 3, 3, 3, 3 },
                    DashStyle = DashStyle.Custom
                })
                {
                    viewPaint.setDrawingRegionRectangle(pen, shape.getRectangle(), g);
                }
               
            }
            else if (shape is MCurve)
            {
                MCurve curve = (MCurve)shape;
                for (int i = 0; i < curve.points.Count; i++)
                {
                    viewPaint.setDrawingCurveSelected(curve.points, new SolidBrush(Color.Blue), g);
                }
            }
            else if (shape is MPolygon)
            {
                MPolygon polygon = (MPolygon)shape;
                for (int i = 0; i < polygon.points.Count; i++)
                {
                    viewPaint.setDrawingCurveSelected(polygon.points, new SolidBrush(Color.Blue), g);
                }
            }
            else
            {
                using (Pen pen = new Pen(Color.Blue, 1)
                {
                    DashPattern = new float[] { 3, 3, 3, 3 },
                    DashStyle = DashStyle.Custom
                })
                {
                    viewPaint.setDrawingRegionRectangle(pen, shape.getRectangle(shape.pointHead, shape.pointTail), g);
                    viewPaint.setDrawingCurveSelected(FindPoints.getControlPoints(shape),
                         new SolidBrush(Color.Blue), g);
                }
            }
        }

        public void updateMovingShape(Point goal, SHAPE shape)
        {
            shape.pointHead = new Point(shape.pointHead.X + goal.X, shape.pointHead.Y + goal.Y);
            shape.pointTail = new Point(shape.pointTail.X + goal.X, shape.pointTail.Y + goal.Y);
            if (isGrRetagle(shape) != -1)
            {
                for (int j = 0; j < dataManager.groups.Count; j++)
                {
                    for (int z = 0; z < dataManager.groups[j].shapeList.Count; z++)
                    {
                        for (int i = 0; i < dataManager.shapeList.Count; i++)
                        {
                            if (dataManager.shapeList[i] == dataManager.groups[j].shapeList[z])
                            {
                                dataManager.shapeList[i].pointHead = new Point(dataManager.shapeList[i].pointHead.X + goal.X, dataManager.shapeList[i].pointHead.Y + goal.Y);
                                dataManager.shapeList[i].pointTail = new Point(dataManager.shapeList[i].pointTail.X + goal.X, dataManager.shapeList[i].pointTail.Y + goal.Y);
                                if (dataManager.shapeList[i].name == "Curve" || dataManager.shapeList[i].name == "Polygon"|| dataManager.shapeList[i].name == "Pen")
                                {
                                    for (int f = 0; f<dataManager.shapeList[i].points.Count; f++)
                                    {
                                        dataManager.shapeList[i].points[f] = new Point(dataManager.shapeList[i].points[f].X + goal.X, dataManager.shapeList[i].points[f].Y + goal.Y);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (shape.name == "Curve" || shape.name == "Polygon"||shape.name =="Pen")
            {
                for (int i = 0; i < shape.points.Count; i++)
                {
                    shape.points[i] = new Point(shape.points[i].X + goal.X, shape.points[i].Y + goal.Y);
                }
            }     
        }

        public int isGrRetagle(SHAPE shape)
        {
            int isgrrect = -1;
            for (int j = 0; j < dataManager.groups.Count; j++)
            {
                if (dataManager.groups[j].grRetangle == shape)
                    isgrrect = j;
            }
            return isgrrect;
        }

        public void moveControlPoint(SHAPE shape, Point pointCurrent, Point previous, int index)
        {                    
            if (shape.name == "Curve")
            {
                int deltaX = pointCurrent.X - previous.X;
                int deltaY = pointCurrent.Y - previous.Y;
                shape.points[index] = new Point(shape.points[index].X + deltaX, shape.points[index].Y + deltaY);
            }
            if (shape.name == "Polygon")
            {
                int deltaX = pointCurrent.X - previous.X;
                int deltaY = pointCurrent.Y - previous.Y;
                shape.points[index] = new Point(shape.points[index].X + deltaX, shape.points[index].Y + deltaY);
            }
            if (isGrRetagle(shape) != -1)
            {
                shape.pointTail = new Point(shape.pointTail.X, shape.pointTail.Y);
            }
            else
            {
                int deltaX = pointCurrent.X - previous.X;
                int deltaY = pointCurrent.Y - previous.Y;
                if (index == 1 || index == 6)
                {
                    shape.pointTail = new Point(shape.pointTail.X, shape.pointTail.Y + deltaY);
                }
                else if (index == 3 || index == 4)
                {
                    shape.pointTail = new Point(shape.pointTail.X + deltaX, shape.pointTail.Y);
                }
                else
                {
                    shape.pointTail = pointCurrent;
                }
            }
        }

        public void onClickStopDrawing(MouseButtons mouse)
        {
            if (mouse == MouseButtons.Right)
            {
                if (dataManager.currentShape.Equals(CurrentShape.Polygon))
                {
                    MPolygon polygon = dataManager.shapeList[dataManager.shapeList.Count - 1] as MPolygon;
                    polygon.points.Remove(polygon.points[polygon.points.Count - 1]);
                    dataManager.isDrawingPolygon = false;
                    FindPoints.setPointHeadTail(polygon);
                }
                else if (dataManager.currentShape.Equals(CurrentShape.Curve))
                {
                    MCurve curve = dataManager.shapeList[dataManager.shapeList.Count - 1] as MCurve;
                    curve.points.Remove(curve.points[curve.points.Count - 1]);
                    dataManager.isDrawingCurve = false;
                    FindPoints.setPointHeadTail(curve);
                }
            }
        }

        public void onClickGroup()
        {
            int count=0;
            foreach (SHAPE shape in dataManager.shapeList)
            {
                if (shape.isSelected && isGrRetagle(shape)==-1)
                {
                    count += 1;
                }
            }
            if(count>=2)
            {
                Group group = new Group();
                dataManager.groups.Add(group);
                foreach (SHAPE shape in dataManager.shapeList)
                {
                    if (shape.isSelected)
                    {
                        group.addEntity(shape);
                        shape.groupnumber = dataManager.groups.Count - 1;
                    }
                }
                group.getgroupRetangle();
                FindPoints.setPointHeadTail(group.grRetangle, group.shapeList);
                dataManager.shapeList.Add(group.grRetangle);
                dataManager.shapeList[dataManager.shapeList.Count - 1].isSelected = true;
                viewPaint.refreshDrawing();
            }            
        }

        public void onClickUnGruop()
        {
            List<Group> groupsCopy = new List<Group>(dataManager.groups);
            List<SHAPE> shapeListCopy = new List<SHAPE>(dataManager.shapeList);
            foreach (SHAPE shape in shapeListCopy)
            {
                if (shape.isSelected && isGrRetagle(shape) != -1)
                {
                    int i = isGrRetagle(shape);
                    foreach (SHAPE shape2 in groupsCopy[i].shapeList)
                    {
                        foreach (SHAPE shape3 in dataManager.shapeList)
                        {
                            if (shape2 == shape3)
                            {
                                shape3.groupnumber = -1;
                            }
                        }
                    }
                    dataManager.groups.Remove(groupsCopy[i]);
                    dataManager.shapeList.Remove(shape);
                }
            }
        }
    }
}
