using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal interface IMovableShape: IShape
    {
        void MoveEntireFigure(int dx, int dy);
        int? FindClosestHandle(int x, int y);
        void MoveHandle(int handleId, int newX, int newY);
    }
}
