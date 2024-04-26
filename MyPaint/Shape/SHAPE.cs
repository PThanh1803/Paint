using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using MyPaint.Find ;

namespace MyPaint.Shape
{
    public abstract class SHAPE : ICloneable
    {
        public Point pointHead { get; set; }
        public Point pointTail { get; set; }
        public Color color { get; set; }
        public Pen pen { get; set; }
        public bool isFill { get; set; }
        public string name { get; set; }
        public bool isSelected { get; set; }
        public int groupnumber { get; set; }

        public List<Point> points;
        public abstract object Clone();
        public abstract void Draw(Graphics g);
        public abstract GraphicsPath graphic { get; }
        public Rectangle getRectangle()
        {
            return new Rectangle(pointHead.X,
                pointHead.Y,
                pointTail.X - pointHead.X,
                pointTail.Y - pointHead.Y);
        }
        
        public Rectangle getRectangle(Point a, Point b)
        {
            if (a.X > b.X)
            {
                int temp = a.X;
                a.X = b.X;
                b.X = temp;
            }
            if (a.Y > b.Y)
            {
                int temp = a.Y;
                a.Y = b.Y;
                b.Y = temp;
            }
            return new Rectangle(a.X, a.Y, b.X - a.X, b.Y - a.Y);
        }

        public virtual int GetpointResize(Point p)
        {
            if (groupnumber != -1)
                return -1;
            List<Point> points = FindPoints.getControlPoints(this);
            for (int i = 0; i < 8; i++)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new Rectangle(points[i].X - 4, points[i].Y - 4, 8, 8));

                if (path.IsVisible(p)) return i;
            }
            
            return -1;
        }
        public virtual void changePoint(int index)
        {
            if (index == 0 || index == 1 || index == 3)
            {
                Point point = pointHead;
                pointHead = pointTail;
                pointTail = point;
            }
            if (index == 2)
            {
                int a = pointTail.X;
                int b = pointHead.Y;
                pointHead = new Point(pointHead.X, pointTail.Y);
                pointTail = new Point(a, b);
            }
            if (index == 5)
            {
                int a = pointHead.X;
                int b = pointTail.Y;
                pointHead = new Point(pointTail.X, pointHead.Y);
                pointTail = new Point(a, b);
            }
        }     
    }
}
