using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    public class Group
    {
        public Group() 
        {
            shapeList = new List<SHAPE>();
        }
        public MRectangle grRetangle { get; set; }
        public List<SHAPE> shapeList { get; set; }
        public void getgroupRetangle()
        {
            grRetangle = new MRectangle
            {
                pointHead = new Point(0, 0),
                pointTail = new Point(0, 0),
                grColor = Color.WhiteSmoke,
                isGr = true
            };
        
        }
        public void addEntity(SHAPE shape)
        {
            shapeList.Add(shape);
        }
    }
}
