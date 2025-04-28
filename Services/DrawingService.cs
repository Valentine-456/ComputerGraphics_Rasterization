using ComputerGraphics_Rasterization.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace ComputerGraphics_Rasterization.Services
{
    internal class DrawingService
    {
        private DrawingMode currentMode = DrawingMode.None;
        private IShape currentShape = null;
        private CanvasService canvasService;

        public DrawingService(CanvasService canvasService)
        {
            this.canvasService = canvasService;
        }

        public void SetDrawingMode(DrawingMode mode)
        {
            currentMode = mode;
            currentShape = null;
        }

        public DrawingMode GetDrawingMode()
        {
            return currentMode;
        }

        public void ClearCurrentShape()
        {
            currentShape = null;
        }

        public void HandleLeftClick(Point click, Color selectedColor, int thickness)
        {
            if (currentShape == null)
            {
                switch (currentMode)
                {
                    case DrawingMode.Line:
                        currentShape = new LineShape((int)click.X, (int)click.Y, (int)click.X, (int)click.Y, selectedColor, thickness);
                        break;
                    case DrawingMode.Circle:
                        currentShape = new CircleShape((int)click.X, (int)click.Y, 0, selectedColor);
                        break;
                    case DrawingMode.RoundedRectangle:
                        currentShape = new RoundedRectangleShape(selectedColor);
                        ((RoundedRectangleShape)currentShape).AddClick((int)click.X, (int)click.Y);
                        break;
                    case DrawingMode.Polygon:
                        currentShape = new PolygonShape(selectedColor, thickness);
                        ((PolygonShape)currentShape).AddVertex((int)click.X, (int)click.Y);
                        break;
                }
            }
            else
            {
                switch (currentShape)
                {
                    case LineShape line:
                        line.X1 = (int)click.X;
                        line.Y1 = (int)click.Y;
                        canvasService.AddShape(currentShape);
                        currentShape = null;
                        break;
                    case CircleShape circle:
                        circle.Radius = (int)Math.Sqrt(Math.Pow(click.X - circle.X, 2) + Math.Pow(click.Y - circle.Y, 2));
                        canvasService.AddShape(currentShape);
                        currentShape = null;
                        break;
                    case RoundedRectangleShape rounded:
                        rounded.AddClick((int)click.X, (int)click.Y);
                        if (rounded.IsComplete)
                        {
                            canvasService.AddShape(rounded);
                            currentShape = null;
                        }
                        break;
                    case PolygonShape polygon:
                        if (!polygon.IsClosed)
                        {
                            if (polygon.CloseToFirstVertex((int)click.X, (int)click.Y))
                            {
                                polygon.Close();
                                canvasService.AddShape(polygon);
                                currentShape = null;
                            }
                            else
                            {
                                polygon.AddVertex((int)click.X, (int)click.Y);
                            }
                        }
                        break;
                }
            }
            canvasService.DrawAll(currentShape);
        }

        public IShape GetCurrentShape() => currentShape;
    }
}
