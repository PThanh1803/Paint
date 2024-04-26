using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MyPaint.Shape;
using MyPaint.CurentShape;


namespace MyPaint.Shape
{
    
    public class DataManager
    {
        public static DataManager instance;

        public List<SHAPE> shapeList { get; set; }

        public SHAPE shapeToMove { get; set; }

        public System.Drawing.Rectangle rectangleRegion;

        public bool isMouseDown { get; set; }

        public bool isMovingShape { get; set; }

        public bool isMovingMouse { get; set; }

        public bool isDrawingCurve { get; set; }

        public bool isDrawingPolygon { get; set; }

        public bool isDrawingPen { get; set; }

        public bool isDrawingEraser { get; set; }

        public bool isFill { get; set; }

        public bool isSave{ get; set; }

        public bool isNotNone{ get; set; }

        public int pointToResize { get; set; }

        public CurrentShape currentShape { get; set; }

        public Point cursorCurrent { get; set; }

        public Color colorCurrent { get; set; }

        public int lineSize { get; set; }
        public String style { get; set; }
        public Pen Pen { get; set; }
        public List<Group> groups { get; set; }
        
        private DataManager()
        {
            groups = new List<Group>();
            shapeList = new List<SHAPE>();
            pointToResize = -1;
        }

        public static DataManager getInstance()
        {
            if (instance == null) instance = new DataManager();
            return instance;
        }
     
        public void updatePointTail(Point p)
        {
            shapeList[shapeList.Count - 1].pointTail = p;
        }

        public void addShape(SHAPE shape)
        {
            shapeList.Add(shape);
        }
        
        public void offAllShapeSelected()
        {
            shapeList.ForEach(shape => shape.isSelected = false);
        }
     
        public Point distanceXY(Point x, Point y)
        {
            return new Point(y.X - x.X, y.Y - x.Y);
        }
      
        public void updateRectangleRegion(Point p)
        {         
            rectangleRegion.Width = p.X - rectangleRegion.X;
            rectangleRegion.Height = p.Y - rectangleRegion.Y;
        }
    }
}
