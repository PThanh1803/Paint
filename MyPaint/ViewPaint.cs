using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using MyPaint.Shape;
using MyPaint.CurentShape;


namespace Mypaint.View
{  
    interface ViewPaint
    {     
        void refreshDrawing();
        
        void setCursor(Cursor cursor);
      
        void setColor(Color color);
       
        void setDrawing(SHAPE shape, Graphics g);
      
        void setDrawingLineSelected(SHAPE shape, Brush brush, Graphics g);
       
        void setDrawingCurveSelected(List<Point> points, Brush brush, Graphics g);
      
        void setDrawingRegionRectangle(Pen p, Rectangle rectangle, Graphics g);
    }
}
