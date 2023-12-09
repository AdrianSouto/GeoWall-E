using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class Intersect : MyExpression, IGraphicObject
{
    public double PosX { get; }
    public double PosY { get; }
    public Geometry MyGeometry { get; }

    public Intersect(List<MyExpression> param)
    {
        IGraphicObject o1 = (IGraphicObject)param[0];
        IGraphicObject o2 = (IGraphicObject)param[1];
        MyGeometry = Geometry.Combine(o1.MyGeometry, o2.MyGeometry, GeometryCombineMode.Intersect, null);
    }
    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        /*if (MyGeometry != null && MyGeometry.GetFlattenedPathGeometry().Figures.Count > 0)
        {
            PathGeometry pathGeometry = MyGeometry.GetFlattenedPathGeometry();
            List<Point> puntosInteseccion = 
                pathGeometry.Figures[0].Segments.OfType<LineSegment>().Select(seg => seg.Point).ToList();
            
            */
        IEnumerable<Point> puntosInteseccion = GetIntesectionPoints(MyGeometry);
            foreach (Point punto in puntosInteseccion)
            {
                Geometry g = new EllipseGeometry(punto, 5, 5);
                Path canvasPoint = new Path
                {
                    Data = g,
                    Fill = color
                };
                canvas.Children.Add(canvasPoint);  
            }
        //}
    }

    private IEnumerable<Point> GetIntesectionPoints(Geometry geometry)
    {
        if (geometry is PathGeometry pathGeometry)
        {
            foreach (var figure in pathGeometry.Figures) 
            {
                foreach (var segment in figure.Segments)
                {
                    if (segment is LineSegment lineSegment)
                    {
                        yield return lineSegment.Point;
                    }
                }
            }
        }
    }

    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }

    public override string value => "intersect";
}