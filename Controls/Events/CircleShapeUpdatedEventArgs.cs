using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Controls.Events
{
    public class CircleShapeUpdatedEventArgs
    {
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? Radius { get; set; }
        public Color? Color { get; set; }
    }
}
