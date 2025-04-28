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
    /// Interaction logic for PolygonTooltab.xaml
    /// </summary>
    public partial class PolygonTooltab : UserControl
    {
        public event EventHandler<PolygonShapeUpdatedEventArgs> PolygonShapeUpdated;
        public int SelectedThickness => (int)ThicknessSlider.Value;
        public Color SelectedColor => ColorPickerControl.SelectedColor ?? Colors.Black;

        public PolygonTooltab()
        {
            InitializeComponent();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PolygonShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs
            {
                Thickness = (int)e.NewValue,
                Color = ColorPickerControl.SelectedColor
            });
        }

        private void ColorPickerControl_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            PolygonShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs
            {
                Thickness = SelectedThickness,
                Color = e.NewValue
            });
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
