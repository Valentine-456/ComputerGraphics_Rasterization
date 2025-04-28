using ComputerGraphics_Rasterization.Controls.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Controls
{
    /// <summary>
    /// Interaction logic for CircleTooltab.xaml
    /// </summary>
    public partial class CircleTooltab : UserControl
    {
        public int SelectedRadius => (int)RadiusSlider.Value;
        public Color SelectedColor => ColorPickerControl.SelectedColor ?? Colors.Black;
        public event EventHandler<CircleShapeUpdatedEventArgs> CircleShapeUpdated;
        public CircleTooltab()
        {
            InitializeComponent();
        }

        public void SetValues(int x, int y, int radius, Color color)
        {
            CenterTextBlock.Text = $"X: {x}, Y: {y}";
            RadiusSlider.Value = radius;
            ColorPickerControl.SelectedColor = color;
        }

        public void ClearValues()
        {
            CenterTextBlock.Text = "";
            RadiusSlider.Value = 10;
            ColorPickerControl.SelectedColor = Colors.Black;
        }

        private void RadiusSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CircleShapeUpdated?.Invoke(this, new CircleShapeUpdatedEventArgs
            {
                Radius = (int)e.NewValue,
                Color = SelectedColor
            });
        }

        private void ColorPickerControl_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            CircleShapeUpdated?.Invoke(this, new CircleShapeUpdatedEventArgs
            {
                Radius = SelectedRadius,
                Color = e.NewValue
            });
        }
    }
}
