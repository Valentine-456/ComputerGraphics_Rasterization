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
    /// Interaction logic for CircleTooltab.xaml
    /// </summary>
    public partial class CircleTooltab : UserControl
    {
        public int SelectedRadius => (int)RadiusSlider.Value;
        public event EventHandler<CircleShapeUpdatedEventArgs> CircleShapeUpdated;
        public CircleTooltab()
        {
            InitializeComponent();
        }

        public void SetValues(int x, int y, int radius)
        {
            CenterTextBlock.Text = $"X: {x}, Y: {y}";
            RadiusSlider.Value = radius;
        }

        public void ClearValues()
        {
            CenterTextBlock.Text = "";
            RadiusSlider.Value = 10;
        }

        private void RadiusSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CircleShapeUpdated?.Invoke(this, new CircleShapeUpdatedEventArgs
            {
                Radius = (int)e.NewValue
            });
        }
    }
}
