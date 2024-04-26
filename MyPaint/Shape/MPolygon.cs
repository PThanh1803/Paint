using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyPaint.Shape
{
    public class MPolygon:SHAPE
    {

        public MPolygon()
        {
            name = "Polygon";
            points = new List<Point>();
            groupnumber = -1;
        }
   
        public override object Clone()
        {
            MPolygon polygon = new MPolygon
            {
                pointHead = pointHead,
                pointTail = pointTail,
                isSelected = isSelected,               
                color = color,
                isFill = isFill
            };
            points.ForEach(point => polygon.points.Add(point));
            return polygon;
        }

        public override void Draw (System.Drawing.Graphics g)
        {
            using (GraphicsPath path = graphic)
            {
                if (!isFill)
                {                 
                     g.DrawPath(pen, path);                   
                }
                else
                {
                    using (Brush brush = new SolidBrush(color))
                    {
                        if (points.Count < 3)
                        {                           
                            g.DrawPath(pen, path);                          
                        }
                        else
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
            }
        }
        public override System.Drawing.Drawing2D.GraphicsPath graphic
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                if (points.Count < 3)
                {
                    path.AddLine(points[0], points[1]);
                }
                else
                {
                    path.AddPolygon(points.ToArray());
                }

                return path;
            }
        }
        public override int GetpointResize(Point p)
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
