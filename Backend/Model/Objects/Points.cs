using System;
using System.Linq;
using System.Windows.Media;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;
using Point = System.Windows.Point;

namespace GeoWalle.Backend.Model.Objects;

public class Points: Sequence
{
    private IGraphicObject figura;

    public Points(IGraphicObject figura) : base(0)
    {
        this.figura = figura;
        MoveNext();
    }
    private Point PuntoAleatorio(IGraphicObject figura)
    {
        PathGeometry pathGeometry = figura.MyGeometry.GetFlattenedPathGeometry();
        Point[] puntos = pathGeometry.Figures.SelectMany(f => f.Segments.OfType<LineSegment>().Select(s => s.Point)).ToArray();
        Random random = new Random();
        return puntos[random.Next() % puntos.Length];
    }

    public override bool MoveNext()
    {
        current = new GPoint(PuntoAleatorio(figura));
        return true;
    }

    private MyExpression current { get; set; }
    public override MyExpression Current => current;
}