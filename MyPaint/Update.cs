using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using Mypaint.View;
using MyPaint.Shape;
using System.Drawing.Drawing2D;


namespace Mypaint.Updates
{
    class Update 
    {
        ViewPaint viewPaint;

        DataManager dataManager;

        public Update(ViewPaint viewPaint)
        {
            this.viewPaint = viewPaint;
            dataManager = DataManager.getInstance();
        }
       
        public void onClickSelectColor(System.Drawing.Color color, Graphics g)
        {                         
            dataManager.colorCurrent = color;
            Pen pen = new Pen(color, dataManager.lineSize);
            if (dataManager.style == "dash")
            {               
                pen.DashStyle = DashStyle.Dash;
            }
            if (dataManager.style == "dot")
            {
                pen.DashStyle = DashStyle.Dot;
            }
            dataManager.Pen = pen;
            viewPaint.setColor(color);
            foreach (SHAPE item in dataManager.shapeList)
            {
                if (item.isSelected)
                {
                   item.pen = dataManager.Pen;
                   item.color = color;
                   viewPaint.setDrawing(item, g);
                    viewPaint.refreshDrawing();
                }
            }                     
        }

        public void onClickSelectSize(int size,Graphics g)
        {
            dataManager.lineSize = size;
            Pen pen = new Pen(dataManager.colorCurrent, dataManager.lineSize);                      
            if (dataManager.style == "dash")
            {
                pen.DashStyle = DashStyle.Dash;
            }
            if (dataManager.style == "dot")
            {
                pen.DashStyle = DashStyle.Dot;
            }
            dataManager.Pen = pen;
            foreach (SHAPE item in dataManager.shapeList)
            {
                if (item.isSelected)
                {
                    item.pen = dataManager.Pen;
                    viewPaint.setDrawing(item, g);
                    viewPaint.refreshDrawing();
                }
            }
        }

        public void selectStyle (Graphics g, String style)
        {        
            Pen pen = new Pen( dataManager.colorCurrent, dataManager.lineSize);
            if(style == "line")
            {
                dataManager.style = style;
                pen = new Pen(dataManager.colorCurrent, dataManager.lineSize);
            }
            if (style=="dash")
            {
                dataManager.style = style;
                pen.DashPattern = new float[] { 2, 2 };
            }
            if (style == "dot")
            {
                dataManager.style = style;
                pen.DashStyle = DashStyle.Dot;
            }
            dataManager.Pen = pen;
            foreach (SHAPE item in dataManager.shapeList)
            {
                if (item.isSelected)
                {
                    item.pen = dataManager.Pen;
                    viewPaint.setDrawing(item, g);
                    viewPaint.refreshDrawing();
                }
            }
        }


        
    }
}
