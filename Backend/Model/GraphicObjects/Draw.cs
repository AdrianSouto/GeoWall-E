using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Objects;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class Draw : MyExpression
{
    private IGraphicObject? gobject;
    public static Canvas canvas;
    private string? label;
    private Sequence? listGObjects;

    public Draw(MyExpression gobject, string label = "")
    {
        this.gobject = (IGraphicObject) gobject;
        this.label = label;
    }

    public Draw(Sequence listGObjects)
    {
        this.listGObjects = listGObjects;
    }
    public override string Evaluate()
    {
        if (listGObjects == null)
        {
            gobject?.Draw(canvas, Brushes.Black);
            DrawLabel();        
        }
        else
        {
            while (listGObjects.MoveNext())
            {
                ((IGraphicObject)listGObjects.Current).Draw(canvas, Brushes.Black);
            }
            
        }
        

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