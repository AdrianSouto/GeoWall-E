using System.Windows.Controls;
using System.Windows.Media;

namespace GeoWalle.Backend.Model.GraphicObjects;

public interface IGraphicObject
{
    public double PosX { get; }
    public double PosY { get; }
    public void Draw(Canvas canvas, SolidColorBrush color);
}