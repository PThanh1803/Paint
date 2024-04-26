using Mypaint.Updates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mypaint.View;
using MyPaint.DRAW;
using MyPaint.Shape;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace MyPaint
{

    public partial class Form1 : Form,ViewPaint 
    {
        private DrawShape DrawShape;
        private Graphics gr;
        private Update Update;

        public Form1()
        {
            InitializeComponent();
            initComponents();
            gr = ptbDrawing.CreateGraphics();
        }

        private void initComponents()
        {
            DrawShape = new DrawShape(this);
            Update = new Update(this);
            Update.onClickSelectColor(Current_Color.BackColor, gr);
            Update.onClickSelectSize(btn_Width_LINE.Value + 1,gr);

        }
        public void refreshDrawing()
        {
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, ptbDrawing, new object[] { true });
            ptbDrawing.Invalidate(); 
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawRectangle();
        }

        private void btn_Edit_Color_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Update.onClickSelectColor(colorDialog.Color, gr);
            }
        }

        private void btn_Width_LINE_Scroll(object sender, EventArgs e)
        {
            Update.onClickSelectSize(btn_Width_LINE.Value + 1,gr);
        }

        private void btnChangeColor_Click(object sender, EventArgs e)
        {
            PictureBox ptb = sender as PictureBox;
            Current_Color.BackColor = ptb.BackColor;
            Update.onClickSelectColor(ptb.BackColor, gr);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnFill_color_Click(object sender, EventArgs e)
        {

        }
        public void setCursor(Cursor cursor)
        {
            ptbDrawing.Cursor = cursor;
        }
       
        public void setColor(Color color)
        {
            Current_Color.BackColor = color;
        }

        public void setColor(System.Windows.Forms.Button btn, Color color)
        {
            btn.BackColor = color;
        }
        public void setDrawing(SHAPE shape, Graphics g)
        {
            shape.Draw(g);
        }
        public void setDrawingRegionRectangle(Pen p, Rectangle rectangle, Graphics g)
        {
            g.DrawRectangle(p, rectangle);
        }
        public void setDrawingLineSelected(SHAPE shape, Brush brush, Graphics g)
        {
            g.FillRectangle(brush, new System.Drawing.Rectangle(shape.pointHead.X - 4, shape.pointHead.Y - 4, 8, 8));
            g.FillRectangle(brush, new System.Drawing.Rectangle(shape.pointTail.X - 4, shape.pointTail.Y - 4, 8, 8));
        }
    
        public void setDrawingCurveSelected(List<Point> points, Brush brush, Graphics g)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                g.FillRectangle(brush, new System.Drawing.Rectangle(points[i].X - 4, points[i].Y - 4, 8, 8));
            }
        }
        private void mouseDown_Event(object sender, MouseEventArgs e)
        {
            DrawShape.onClickMouseDown(e.Location);
        }

        //Sự kiện di chuyển chuột, gửi yêu cầu xử lý di chuyển chuột đến presenter
        private void mouseMove_Event(object sender, MouseEventArgs e)
        {
            
            DrawShape.onClickMouseMove(e.Location);
        }
        private void onPaint_Event(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            DrawShape.getDrawing(e.Graphics);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DrawShape.onClickSelect();
        }

        private void mouseUp_Event(object sender, MouseEventArgs e)
        {
            DrawShape.onClickMouseUp();
        }

        private void btnFill_Retangle_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawFillRectangle();
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawEllipse();
        }

        private void btnFill_Ellipse_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawFillEllipse();
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawLine();
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawPolygon();
        }

        private void ptbDrawing_MouseClick(object sender, MouseEventArgs e)
        {
            DrawShape.onClickStopDrawing(e.Button);
        }

        private void btnFill_Polygon_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawFillPolygon();
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawCurve();
        }

        private void btnCurve_MouseClick(object sender, MouseEventArgs e)
        {
            DrawShape.onClickStopDrawing(e.Button);
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            DrawShape.onClickDrawPen();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DrawShape.deleteShape();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            DrawShape.eventKeyDown(ptbDrawing, e.KeyCode);
        }

        private void btnFill_color_Click_1(object sender, EventArgs e)
        {
            DrawShape.fillShape();
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            DrawShape.onClickGroup();
        }

        private void btnUngroup_Click(object sender, EventArgs e)
        {
            DrawShape.onClickUnGruop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String style;
            string selectedItem = cbbStyle.SelectedItem.ToString();
            switch (selectedItem)
            {
                case "___________________________":
                    {
                         style = "line";
                        Update.selectStyle(gr,style);
                    }
                    break;
                case "- - - - - - - - - - - - - - - - -":
                    {
                        style = "dash";
                        Update.selectStyle(gr,style);
                    }
                    break;
                case ". . . . . . . . . . . . . .":
                    {
                        style = "dot";
                        Update.selectStyle(gr, style);
                    }
                    break;
                default:
                    break;
            }
        }
    }
} 
