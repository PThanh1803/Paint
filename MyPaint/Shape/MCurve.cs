using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyPaint.Shape
{
    public class MCurve:SHAPE
    {
        public MCurve() 
        {
            points = new List<Point>();
            name = "Curve";
            groupnumber = -1;
        }
      
        public override object Clone()
        {
            MCurve curve = new MCurve
            {
                pointHead = pointHead,
                pointTail = pointTail,
                isSelected = isSelected,              
                color = color,              
            };
            points.ForEach(point => curve.points.Add(point));
            return curve;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            using (GraphicsPath path = graphic)
            {
                g.DrawPath(pen, path);
            }
        }

        public override System.Drawing.Drawing2D.GraphicsPath graphic
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddCurve(points.ToArray());
                return path;
            }
        }

        public override int GetpointResize(System.Drawing.Point p)
        {
            if (groupnumber != -1)
                return -1;
            for (int i = 0; i < points.Count; i++)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new Rectangle(points[i].X - 4, points[i].Y - 4, 8, 8));
                if (path.IsVisible(p)) return i;
            }
           
            return -1;
        }
    }
}
