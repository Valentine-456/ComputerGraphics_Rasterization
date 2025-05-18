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
using ComputerGraphics_Rasterization.Controls.Events;

namespace ComputerGraphics_Rasterization.Controls
{
    /// <summary>
    /// Interaction logic for RectangleTooltab.xaml
    /// </summary>
    public partial class RectangleTooltab : UserControl
    {
        public event EventHandler<PolygonShapeUpdatedEventArgs> RectangleShapeUpdated;

        public int SelectedThickness => (int)ThicknessSlider.Value;
        public Color SelectedColor => ColorPickerControl.SelectedColor ?? Colors.Black;

        public RectangleTooltab()
        {
            InitializeComponent();

            ThicknessSlider.ValueChanged += (s, e) =>
                RectangleShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs { Thickness = (int)e.NewValue });

            ColorPickerControl.SelectedColorChanged += (s, e) =>
                RectangleShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs { Color = e.NewValue });
        }

        public void SetValues(int thickness, Color color)
        {
            ThicknessSlider.Value = thickness;
            ColorPickerControl.SelectedColor = color;
        }

        public void ClearValues()
        {
            ThicknessSlider.Value = 1;
            ColorPickerControl.SelectedColor = Colors.Black;
        }
    }
}
