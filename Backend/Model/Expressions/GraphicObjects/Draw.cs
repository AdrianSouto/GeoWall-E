using System;
using System.Windows.Controls;
using System.Windows.Media;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;
using Color = GeoWalle.Backend.Model.Objects.Color;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class Draw : MyExpression
{
    private IGraphicObject? gobject;
    public static Canvas canvas;
    public static ScrollViewer CanvasScrollViewer;
    private string? label;

    public Draw(MyExpression gobject, string label = "")
    {
        this.gobject = (IGraphicObject) gobject;
        this.label = label;
    }
    public override string Evaluate()
    {
        gobject?.Draw(canvas,Color.GetColor());
        DrawLabel();        
        

        return "";
    }

    private void DrawLabel()
    {
        TextBlock label = new TextBlock
        {
            Text = this.label,
            Foreground = Brushes.Black
        };
        Canvas.SetLeft(label, gobject.PosX+10);
        Canvas.SetTop(label, gobject.PosY);
        canvas.Children.Add(label);
    }

    public override string value => "Draw";
}