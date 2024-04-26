using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    public class MPen:MCurve
    {
        public MPen() 
        {
            name = "Pen";
            groupnumber = -1;
        }

        public bool isEraser { get; set; }
    }
}
