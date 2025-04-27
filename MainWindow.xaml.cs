using ComputerGraphics_Rasterization.Controls;
using ComputerGraphics_Rasterization.RenderLogic;
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

namespace ComputerGraphics_Rasterization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap bitmap;
        private CanvasService canvasService;

        private bool isDrawing = false;
        private IShape currentShape = null;
        private IShape selectedShape = null;

        private LineTooltab _lineTooltab = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCanvas();
        }

        private void InitializeCanvas()
        {
            int width = 1000;
            int height = 1000;
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            DrawingSurface.Source = bitmap;

            CanvasRenderer canvasRenderer = new CanvasRenderer(bitmap);
            canvasService = new CanvasService(canvasRenderer);
        }

        private void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point click = e.GetPosition(DrawingSurface);

            if (currentShape == null && selectedShape == null)
            {
                int thickness = _lineTooltab?.SelectedThickness ?? 1;
                currentShape = new LineShape((int)click.X, (int)click.Y, (int)click.X, (int)click.Y, Colors.Black, thickness);
            }
            else if (currentShape != null)
            {
                if (currentShape is LineShape line)
                {
                    line.X1 = (int)click.X;
                    line.Y1 = (int)click.Y;
                }
                canvasService.AddShape(currentShape);
                UpdateShapesList();
                currentShape = null;
                RedrawCanvas();
            }
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (currentShape != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point move = e.GetPosition(DrawingSurface);

                if (currentShape is LineShape line)
                {
                    line.X1 = (int)move.X;
                    line.Y1 = (int)move.Y;
                }
                RedrawCanvas();
            }
        }

        private void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // optional logic if needed
        }

        private void OnCanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point click = e.GetPosition(DrawingSurface);
            selectedShape = canvasService.FindShapeAt((int)click.X, (int)click.Y);
            if (selectedShape != null)
            {
                _lineTooltab.SetValues(0, 0, 0, 0, 0);
            }
            UpdateLineTooltab();
        }

        private void RedrawCanvas()
        {
            canvasService.DrawAll();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            canvasService.ClearCanvas();
            ShapesListBox.Items.Clear();
            currentShape = null;
            selectedShape = null;
        }

        private void LineTooltab_Click(object sender, RoutedEventArgs e)
        {
            _lineTooltab = new LineTooltab();
            _lineTooltab.DeleteButton.Click += OnDeleteSelectedLineClicked;
            ToolTab.Content = _lineTooltab;
        }

        private void OnDeleteSelectedLineClicked(object sender, RoutedEventArgs e)
        {
            if (selectedShape != null)
            {
                canvasService.RemoveShape(selectedShape);
                UpdateShapesList();
                RedrawCanvas();
                selectedShape = null;
            }
        }

        private void UpdateLineTooltab()
        {
            if (_lineTooltab != null && selectedShape is LineShape line)
            {
                _lineTooltab.SetValues(line.X0, line.Y0, line.X1, line.Y1, line.Thickness);
            }
        }

        private void UpdateShapesList()
        {
            ShapesListBox.ItemsSource = null;
            ShapesListBox.ItemsSource = canvasService.Shapes
                .OrderBy(shape => shape.ZIndex)
                .Select(shape => $"Line Z-Index: {shape.ZIndex}");
        }

        private void OnShapeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ShapesListBox.SelectedIndex >= 0)
            {
                var selectedLineId = (uint)ShapesListBox.SelectedIndex;
                selectedShape = canvasService.Shapes.FirstOrDefault(s => s.ZIndex == selectedLineId);
                UpdateLineTooltab();
                RedrawCanvas();
            }
        }

    }
}
