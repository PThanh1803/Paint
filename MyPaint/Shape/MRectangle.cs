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
    public class MRectangle: SHAPE 
    {
        public bool isGr { get; set; }  
        public Color grColor { get; set; }
        public MRectangle()
        {
            name = "retangle";
            groupnumber = -1;
        }

        public override object Clone()
        {
            return new MRectangle
            {
                pointHead = pointHead,
                pointTail = pointTail,
                isSelected = isSelected,
                color = color,
            };
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            using (GraphicsPath path = graphic)
            {
                if (isFill)
                {
                    using (Brush b = new SolidBrush(color))
                    {
                        g.FillPath(b, path);
                    }
                }
                if (isGr)
                {
                    using (Pen pen = new Pen(grColor, 1)
                    {
                        DashPattern = new float[] { 3, 3, 3, 3 },
                        DashStyle = DashStyle.Custom,                      
                    })
                    {
                        g.DrawPath(pen, path);
                    }                 
                }

                else
                {
                    g.DrawPath(pen , path);                   
                }
            }
        }

        public override GraphicsPath graphic
        {
            get
            {
                GraphicsPath path = new GraphicsPath();

                if (pointTail.X < pointHead.X && pointTail.Y < pointHead.Y)
                {
                    path.AddRectangle(new System.Drawing.Rectangle(pointTail,
                        new Size(pointHead.X - pointTail.X, pointHead.Y - pointTail.Y)));
                }
                else if (pointHead.X > pointTail.X && pointHead.Y < pointTail.Y)
                {
                    path.AddRectangle(new System.Drawing.Rectangle(new Point(pointTail.X, pointHead.Y),
                        new Size(pointHead.X - pointTail.X, pointTail.Y - pointHead.Y)));
                }
                else if (pointHead.X < pointTail.X && pointHead.Y > pointTail.Y)
                {
                    path.AddRectangle(new System.Drawing.Rectangle(new Point(pointHead.X, pointTail.Y),
                        new Size(pointTail.X - pointHead.X, pointHead.Y - pointTail.Y)));
                }
                else
                {
                    path.AddRectangle(new System.Drawing.Rectangle(pointHead,
                        new Size(pointTail.X - pointHead.X, pointTail.Y - pointHead.Y)));
                }

                return path;
            }
        }

    }
}
