using ComputerGraphics_Rasterization.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ComputerGraphics_Rasterization.Services
{
    internal class FileOperationsService
    {
        public void SaveToFile(string filePath, List<IShape> shapes)
        {
            var shapeDtos = shapes.Select(shape =>
            {
                if (shape is LineShape line)
                {
                    return new ShapeDto
                    {
                        Type = "Line",
                        X0 = line.X0,
                        Y0 = line.Y0,
                        X1 = line.X1,
                        Y1 = line.Y1,
                        Thickness = line.Thickness,
                        Color = line.Color.ToString(),
                        ZIndex = line.ZIndex
                    };
                }
                else if (shape is CircleShape circle)
                {
                    return new ShapeDto
                    {
                        Type = "Circle",
                        X0 = circle.X,
                        Y0 = circle.Y,
                        Radius = circle.Radius,
                        Color = circle.Color.ToString(),
                        ZIndex = circle.ZIndex
                    };
                }
                return null;
            }).Where(dto => dto != null).ToList();

            var json = JsonConvert.SerializeObject(shapeDtos, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public List<IShape> LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var shapeDtos = JsonConvert.DeserializeObject<List<ShapeDto>>(json);

            var shapes = new List<IShape>();
            foreach (var dto in shapeDtos)
            {
                if (dto.Type == "Line")
                {
                    var line = new LineShape(
                        dto.X0 ?? 0,
                        dto.Y0 ?? 0,
                        dto.X1 ?? 0,
                        dto.Y1 ?? 0,
                        (Color)ColorConverter.ConvertFromString(dto.Color),
                        dto.Thickness ?? 1,
                        dto.ZIndex ?? 0
                    );
                    shapes.Add(line);
                }
                else if (dto.Type == "Circle")
                {
                    var circle = new CircleShape(
                        dto.X0 ?? 0,
                        dto.Y0 ?? 0,
                        dto.Radius ?? 0,
                        (Color)ColorConverter.ConvertFromString(dto.Color),
                        dto.ZIndex ?? 0
                    );
                    shapes.Add(circle);
                }
            }

            return shapes;
        }

        private class ShapeDto
        {
            public string Type { get; set; }
            public int? X0 { get; set; }
            public int? Y0 { get; set; }
            public int? X1 { get; set; }
            public int? Y1 { get; set; }
            public int? Radius { get; set; }
            public int? Thickness { get; set; }
            public string Color { get; set; }
            public uint? ZIndex { get; set; }
        }
    }
}
