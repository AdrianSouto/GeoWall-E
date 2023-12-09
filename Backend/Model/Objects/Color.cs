using System;
using System.Collections.Generic;
using System.Windows.Media;
using GeoWalle.Backend.Model.MyExceptions;

namespace GeoWalle.Backend.Model.Objects;

public static class Color
{
    private static Stack<SolidColorBrush> colors = new();

    public static void ChangeColor(string color)
    {
        switch (color)
        {
            case "black":
                colors.Push(Brushes.Black);
                break;
            case "blue":
                colors.Push(Brushes.Blue);
                break;
            case "red":
                colors.Push(Brushes.Red);
                break;
            case "green":
                colors.Push(Brushes.Green);
                break;
            case "cyan":
                colors.Push(Brushes.Cyan);
                break;
            case "magenta":
                colors.Push(Brushes.Magenta);
                break;
            case "white":
                colors.Push(Brushes.White);
                break;
            case "gray":
                colors.Push(Brushes.Gray);
                break;
            default:
                throw new SyntaxException("El color "+color+" no esta definido");
        }
    }

    public static void RestoreColor()
    {
        if(colors.Count > 1)
            colors.Pop();
    }

    public static SolidColorBrush GetColor()
    {
        return colors.Peek();
    }
}