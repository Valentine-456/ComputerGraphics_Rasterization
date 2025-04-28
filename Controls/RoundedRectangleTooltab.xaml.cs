using ComputerGraphics_Rasterization.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerGraphics_Rasterization.Controls
{
    /// <summary>
    /// Interaction logic for RoundedRectangleTooltab.xaml
    /// </summary>
    public partial class RoundedRectangleTooltab : UserControl
    {
        public event EventHandler<RoundedRectShapeUpdatedEventArgs> RoundedRectShapeUpdated;
        public Color SelectedColor => ColorPickerControl.SelectedColor ?? Colors.Black;
        public RoundedRectangleTooltab()
        {
            InitializeComponent();
        }

        private void ColorPickerControl_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            RoundedRectShapeUpdated?.Invoke(this, new RoundedRectShapeUpdatedEventArgs
            {
                Color = e.NewValue
            });
        }

        public void SetValues(Color color)
        {
            ColorPickerControl.SelectedColor = color;
        }

        public void ClearValues()
        {
            ColorPickerControl.SelectedColor = Colors.Black;
        }
    }
}
