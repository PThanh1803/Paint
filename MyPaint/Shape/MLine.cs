using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    public class MLine:SHAPE
    {
        public MLine()
        { 
            name = "line";
            groupnumber = -1;
        }
        public override object Clone()
        {
            return new MLine
            {
                pointHead = pointHead,
                pointTail = pointTail,
                isSelected = isSelected,
                color = color,
            };
        }

        public override GraphicsPath graphic
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                if(pointHead.X > pointTail.X)
                    path.AddLine(pointHead, pointTail);
                else if (pointHead.X < pointTail.X)
                    path.AddLine(pointTail, pointHead); 
                return path;
            }          
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            using (GraphicsPath path = graphic)
            {               
                g.DrawPath(pen, path);               
            }
        }
    }
}
