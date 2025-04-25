using ComputerGraphics_Rasterization.RenderLogic;
using ComputerGraphics_Rasterization.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics_Rasterization.Services
{
    internal class CanvasService
    {
        public List<IShape> Shapes { get; private set; } = new List<IShape>();
        private CanvasRenderer Renderer;

        public CanvasService(CanvasRenderer renderer)
        {
            Renderer = renderer;
        }

        void AddShape(IShape shape)
        {
            Shapes.Add(shape);
        }

        public void RemoveShape(IShape shape)
        {
            Shapes.Remove(shape);
        }

        public void DrawAll()
        {
            foreach (var shape in Shapes.OrderBy(s => s.ZIndex))
                shape.Draw(Renderer);
        }

        public IShape FindShapeAt(int x, int y)
        {
            return Shapes.LastOrDefault(shape => shape.IsTargeted(x, y));
        }
    }
}
