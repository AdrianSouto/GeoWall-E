using System.Windows.Controls;
using System.Windows.Media;
using GeoWalle.Backend.Model.Expressions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class Draw : MyExpression
{
    private IGraphicObject GObjects;
    public static Canvas canvas;
    private string label;

    public Draw(MyExpression GObjects, string label = "")
    {
        this.GObjects = (IGraphicObject) GObjects;
        this.label = label;
    }
    public override string Evaluate()
    {
        
            GObjects.Draw(canvas, Brushes.Black);
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
        Canvas.SetLeft(label, GObjects.PosX+10);
        Canvas.SetTop(label, GObjects.PosY);
        canvas.Children.Add(label);
    }

    public override string value => "Draw";
}