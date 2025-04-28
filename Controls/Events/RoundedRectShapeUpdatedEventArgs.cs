using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Controls.Events
{
    public class RoundedRectShapeUpdatedEventArgs: EventArgs
    {
        public Color? Color { get; set; }
    }
}
