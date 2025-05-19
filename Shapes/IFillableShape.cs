using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ComputerGraphics_Rasterization.RenderLogic;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal interface IFillableShape: IMovableShape
    {
        bool IsFilled { get; set; }
        Color? FillColor { get; set; }
        string FillImagePath { get; set; }
        void Fill(CanvasRenderer renderer, Point? seed = null);
        //Point? FillingSeed { get; set; }

    }
}
