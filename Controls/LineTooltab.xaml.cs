using ComputerGraphics_Rasterization.Controls.Events;
using ComputerGraphics_Rasterization.Services;
using ComputerGraphics_Rasterization.Shapes;
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
    /// Interaction logic for LineTooltab.xaml
    /// </summary>
    public partial class LineTooltab : UserControl
    {
        public int SelectedThickness => (int)ThicknessSlider.Value;
        public event EventHandler<LineShapeUpdatedEventArgs> LineShapeUpdated;

        public LineTooltab()
        {
            InitializeComponent();
        }

        public void SetValues(int x0, int y0, int x1, int y1, int thickness)
        {
            StartPointTextBlock.Text = $"X: {x0}, Y: {y0}";
            EndPointTextBlock.Text = $"X: {x1}, Y: {y1}";
            ThicknessSlider.Value = thickness;
        }

        public void ClearValues()
        {
            StartPointTextBlock.Text = $"";
            EndPointTextBlock.Text = $"";
            ThicknessSlider.Value = 1;
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LineShapeUpdated?.Invoke(this, new LineShapeUpdatedEventArgs
            {
                Thickness = (int)e.NewValue
            });
        }
    }
}
