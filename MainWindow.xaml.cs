using ComputerGraphics_Rasterization.Controls;
using ComputerGraphics_Rasterization.Controls.Events;
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
        private int? draggingHandleId = null;
        private Point? lastDragPoint = null;


        private LineTooltab _lineTooltab = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCanvas();
            LineTooltab_Click(this, null);
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

        private void LineTooltab_Click(object sender, RoutedEventArgs e)
        {
            _lineTooltab = new LineTooltab();
            _lineTooltab.DeleteButton.Click += OnDeleteSelectedLineClicked;
            _lineTooltab.LineShapeUpdated += OnLineShapeUpdated;
            ToolTab.Content = _lineTooltab;
        }

        private void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point click = e.GetPosition(DrawingSurface);

            if (currentShape == null)
            {
                int thickness = _lineTooltab?.SelectedThickness ?? 1;
                currentShape = new LineShape((int)click.X, (int)click.Y, (int)click.X, (int)click.Y, Colors.Black, thickness);
            }
            else
            {
                if (currentShape is LineShape line)
                {
                    line.X1 = (int)click.X;
                    line.Y1 = (int)click.Y;
                }
                canvasService.AddShape(currentShape);
                UpdateShapesList();
                currentShape = null;
                canvasService.DrawAll();
            }
        }

        private void OnCanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point click = e.GetPosition(DrawingSurface);
            selectedShape = canvasService.FindShapeAt((int)click.X, (int)click.Y);

            if (selectedShape is IMovableShape movable)
            {
                draggingHandleId = movable.FindClosestHandle((int)click.X, (int)click.Y);
                lastDragPoint = click;
            }

            UpdateLineTooltab();
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            Point move = e.GetPosition(DrawingSurface);

            if (selectedShape != null && e.RightButton == MouseButtonState.Pressed && selectedShape is IMovableShape movable)
            {
                if (draggingHandleId.HasValue)
                {
                    movable.MoveHandle(draggingHandleId.Value, (int)move.X, (int)move.Y);
                }
                else if (lastDragPoint.HasValue)
                {
                    int dx = (int)(move.X - lastDragPoint.Value.X);
                    int dy = (int)(move.Y - lastDragPoint.Value.Y);
                    movable.MoveEntireFigure(dx, dy);
                    lastDragPoint = move;
                }
                canvasService.DrawAll();
            }
        }

        private void OnCanvasMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedShape != null)
            {
                UpdateLineTooltab();
            }
            draggingHandleId = null;
            lastDragPoint = null;
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            ShapesListBox.ItemsSource = null;
            canvasService.ClearCanvas();
            canvasService.DrawAll();
            currentShape = null;
            selectedShape = null;
        }

        private void OnDeleteSelectedLineClicked(object sender, RoutedEventArgs e)
        {
            if (selectedShape != null)
            {
                canvasService.RemoveShape(selectedShape);
                UpdateShapesList();
                _lineTooltab.ClearValues();
                canvasService.DrawAll();
                selectedShape = null;
            }
        }

        private void UpdateLineTooltab()
        {
            if (_lineTooltab != null)
            {
                if (selectedShape is LineShape line)
                {
                    _lineTooltab.SetValues(line.X0, line.Y0, line.X1, line.Y1, line.Thickness);
                }
                else
                {
                    _lineTooltab.ClearValues();
                }
            }
        }

        private void OnLineShapeUpdated(object sender, LineShapeUpdatedEventArgs e)
        {
            if (selectedShape is LineShape line)
            {
                if (e.Thickness.HasValue)
                    line.Thickness = e.Thickness.Value;

                if (e.X0.HasValue)
                    line.X0 = e.X0.Value;

                if (e.Y0.HasValue)
                    line.Y0 = e.Y0.Value;

                if (e.X1.HasValue)
                    line.X1 = e.X1.Value;

                if (e.Y1.HasValue)
                    line.Y1 = e.Y1.Value;

                if (e.Color.HasValue)
                    line.Color = e.Color.Value;

                canvasService.DrawAll();
            }
        }

        private void UpdateShapesList()
        {
            ShapesListBox.ItemsSource = null;
            ShapesListBox.ItemsSource = canvasService
                .Shapes
                .OrderBy(s => s.ZIndex)
                .ToList();
        }

        private void OnShapeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ShapesListBox.SelectedItem is IShape shape)
            {
                selectedShape = shape;
                UpdateLineTooltab();
                canvasService.DrawAll();
            }
            else
            {
                selectedShape = null;
                _lineTooltab?.ClearValues();
            }
        }
    }
}
