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
        public bool? IsFilled { get; set; }
        public Color FillColor => FillColorPicker.SelectedColor ?? Colors.White;


        public PolygonTooltab()
        {
            InitializeComponent();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PolygonShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs
            {
                Thickness = (int)e.NewValue,
                Color = ColorPickerControl.SelectedColor,
                IsFilled = FillCheckBox.IsChecked == true,
                FillColor = FillColorPicker.SelectedColor
            });
        }

        private void ColorPickerControl_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (sender == ColorPickerControl)
            {
                PolygonShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs
                {
                    Color = e.NewValue,
                    Thickness = (int)ThicknessSlider.Value,
                    IsFilled = FillCheckBox.IsChecked == true,
                    FillColor = FillColorPicker.SelectedColor
                });
            }
            else if (sender == FillColorPicker)
            {
                PolygonShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs
                {
                    FillColor = e.NewValue,
                    IsFilled = FillCheckBox.IsChecked == true,
                    Thickness = (int)ThicknessSlider.Value,
                    Color = ColorPickerControl.SelectedColor
                });
            }
        }

        private void FillCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            PolygonShapeUpdated?.Invoke(this, new PolygonShapeUpdatedEventArgs
            {
                FillColor = FillColorPicker.SelectedColor,
                IsFilled = FillCheckBox.IsChecked == true,
                Thickness = (int)ThicknessSlider.Value,
                Color = ColorPickerControl.SelectedColor
            });
        }

        public void SetValues(int thickness, Color color, bool isFilled, Color? fillColor)
        {
            ThicknessSlider.Value = thickness;
            ColorPickerControl.SelectedColor = color;
            FillCheckBox.IsChecked = isFilled;
            FillColorPicker.SelectedColor = fillColor;
        }

        public void ClearValues()
        {
            ThicknessSlider.Value = 1;
            ColorPickerControl.SelectedColor = Colors.Black;
            FillColorPicker.SelectedColor = Colors.White;
        }
    }
}
