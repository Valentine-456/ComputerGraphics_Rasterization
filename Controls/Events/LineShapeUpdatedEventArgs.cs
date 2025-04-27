using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Controls.Events
{
    public class LineShapeUpdatedEventArgs : EventArgs
    {
        public int? X0 { get; set; }
        public int? Y0 { get; set; }
        public int? X1 { get; set; }
        public int? Y1 { get; set; }
        public int? Thickness { get; set; }
        public Color? Color { get; set; }
    }
}
