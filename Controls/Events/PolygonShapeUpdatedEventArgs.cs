using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Controls.Events
{
    public class PolygonShapeUpdatedEventArgs
    {
        public int? Thickness { get; set; }
        public Color? Color { get; set; }

    }
}
