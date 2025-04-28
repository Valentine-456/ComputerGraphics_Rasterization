using ComputerGraphics_Rasterization.Controls;
using ComputerGraphics_Rasterization.Controls.Events;
using ComputerGraphics_Rasterization.RenderLogic;
using ComputerGraphics_Rasterization.Services;
using ComputerGraphics_Rasterization.Shapes;
using Microsoft.Win32;
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
        private DrawingService drawingService;
        private FileOperationsService fileService = new FileOperationsService();

        private IShape selectedShape = null;
        private int? draggingHandleId = null;
        private Point? lastDragPoint = null;

        private LineTooltab _lineTooltab = null;
        private CircleTooltab _circleTooltab = null;
        private RoundedRectangleTooltab _roundedRectangleTooltab = null;

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
            drawingService = new DrawingService(canvasService);
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json";
            saveFileDialog.DefaultExt = "json";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileService.SaveToFile(saveFileDialog.FileName, canvasService.Shapes);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                var loadedShapes = fileService.LoadFromFile(openFileDialog.FileName);
                canvasService.ClearCanvas();
                canvasService.Shapes.AddRange(loadedShapes);
                UpdateShapesList();
                canvasService.DrawAll();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LineTooltab_Click(object sender, RoutedEventArgs e)
        {
            ToolTab.Content = null;
            _circleTooltab = null;
            _roundedRectangleTooltab = null;

            drawingService.SetDrawingMode(DrawingMode.Line);

            _lineTooltab = new LineTooltab();
            _lineTooltab.DeleteButton.Click += OnDeleteSelectedLineClicked;
            _lineTooltab.LineShapeUpdated += OnLineShapeUpdated;
            ToolTab.Content = _lineTooltab;
        }

        private void CircleTooltab_Click(Object sender, RoutedEventArgs e)
        {
            ToolTab.Content = null;
            _lineTooltab = null;
            _roundedRectangleTooltab = null;

            drawingService.SetDrawingMode(DrawingMode.Circle);

            _circleTooltab = new CircleTooltab();
            _circleTooltab.DeleteButton.Click += OnDeleteSelectedCircleClicked;
            _circleTooltab.CircleShapeUpdated += OnCircleShapeUpdated;
            ToolTab.Content = _circleTooltab;
        }

        private void RoundedRectangleTooltab_Click(Object sender, RoutedEventArgs e)
        {
            ToolTab.Content = null;
            _lineTooltab = null;
            _circleTooltab = null;

            drawingService.SetDrawingMode(DrawingMode.RoundedRectangle);

            _roundedRectangleTooltab = new RoundedRectangleTooltab();
            _roundedRectangleTooltab.DeleteButton.Click += OnDeleteSelectedRoundRectClicked;
            // _roundedRectangleTooltab.CircleShapeUpdated += OnCircleShapeUpdated;
            ToolTab.Content = _roundedRectangleTooltab;
        }

        private void ToggleAntialiasing_Click(object sender, RoutedEventArgs e)
        {
            SettingsService.IsAntialiasingEnabled = !SettingsService.IsAntialiasingEnabled;
            canvasService.DrawAll();
        }

        private void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point click = e.GetPosition(DrawingSurface);
            int thickness = _lineTooltab?.SelectedThickness ?? 1;
            Color color = Colors.Black;

            drawingService.HandleLeftClick(click, color, thickness);

            UpdateShapesList();
            canvasService.DrawAll();
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
            if (selectedShape is LineShape)
            {
                LineTooltab_Click(this, null);
                UpdateLineTooltab();
            }
            else if (selectedShape is CircleShape)
            {
                CircleTooltab_Click(this, null);
                UpdateCircleTooltab();
            }
            else if (selectedShape is RoundedRectangleShape)
            {
                RoundedRectangleTooltab_Click(this, null);
                UpdateCircleTooltab();
            }
            else
            {
                _lineTooltab?.ClearValues();
                _circleTooltab?.ClearValues();
            }
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
            drawingService.ClearCurrentShape();
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

        private void OnDeleteSelectedCircleClicked(object sender, RoutedEventArgs e)
        {
            if (selectedShape != null)
            {
                canvasService.RemoveShape(selectedShape);
                UpdateShapesList();
                _circleTooltab.ClearValues();
                canvasService.DrawAll();
                selectedShape = null;
            }
        }

        private void OnDeleteSelectedRoundRectClicked(object sender, RoutedEventArgs e)
        {
            if (selectedShape != null)
            {
                canvasService.RemoveShape(selectedShape);
                UpdateShapesList();
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

        private void UpdateCircleTooltab()
        {
            if (_circleTooltab != null)
            {
                if (selectedShape is CircleShape circle)
                {
                    _circleTooltab.SetValues(circle.X, circle.Y, circle.Radius);
                }
                else
                {
                    _circleTooltab.ClearValues();
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

        private void OnCircleShapeUpdated(object sender, CircleShapeUpdatedEventArgs e)
        {
            if (selectedShape is CircleShape circle)
            {
                if (e.Radius.HasValue)
                    circle.Radius = e.Radius.Value;

                if (e.X.HasValue)
                    circle.X = e.X.Value;

                if (e.Y.HasValue)
                    circle.Y = e.Y.Value;

                if (e.Color.HasValue)
                    circle.Color = e.Color.Value;

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

                if (shape is LineShape line)
                {
                    drawingService.SetDrawingMode(DrawingMode.Line);
                    LineTooltab_Click(this, null);
                    UpdateLineTooltab();
                }
                else if (shape is CircleShape circle)
                {
                    drawingService.SetDrawingMode(DrawingMode.Circle);
                    CircleTooltab_Click(this, null);
                    UpdateCircleTooltab();
                }
                else if (shape is RoundedRectangleShape roundedRect)
                {
                    drawingService.SetDrawingMode(DrawingMode.RoundedRectangle);
                    RoundedRectangleTooltab_Click(this, null);
                }

                canvasService.DrawAll();
            }
            else
            {
                selectedShape = null;
                _lineTooltab?.ClearValues();
                _circleTooltab?.ClearValues();
            }
        }
    }
}
