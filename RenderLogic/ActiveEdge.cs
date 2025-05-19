using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class ActiveEdge
    {
        public double X { get; set; }
        public double InvSlope { get; set; }
        public int YMax { get; set; }

        public ActiveEdge(double x, double invSlope, int yMax)
        {
            X = x;
            InvSlope = invSlope;
            YMax = yMax;
        }
    }

}
