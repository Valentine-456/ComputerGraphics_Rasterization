using ComputerGraphics_Rasterization.RenderLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal interface IShape
    {
        uint ZIndex { get; set; }
        void Draw(CanvasRenderer renderer);
        bool IsTargeted(int x, int y);
    }
}
