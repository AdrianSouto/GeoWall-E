using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;

namespace GeoWalle.Backend;

public static class Utils
{
    public static bool IsBalanced(string input)
    {
        Dictionary<char, char> specials = new();
        specials.Add('{','}');
        specials.Add('(',')');
        Stack<char> stack = new Stack<char>();
        foreach (char c in input)
        {
            if(specials.Keys.Contains(c)){
                stack.Push(c);
            }else if (specials.Values.Contains(c))
            {
                if (stack.Count == 0)
                    return false;
                char lastChar = stack.Pop();
                if (specials[lastChar] != c)
                    return false;
            }
        }
        return stack.Count == 0;
    }
    public static IEnumerator<GPoint> LineLineIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
    {
        double ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
        double ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

        if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
        {
            // Calcular las pendientes de las rectas
            double m1 = (y2 - y1) / (x2 - x1);
            double m2 = (y4 - y3) / (x4 - x3);

            // Calcular las intersecciones en x
            double xIntersection = (m1 * x1 - m2 * x3 + y3 - y1) / (m1 - m2);

            // Calcular las intersecciones en y
            double yIntersection = m1 * (xIntersection - x1) + y1;

            yield return new GPoint(new Point(xIntersection, yIntersection));
        }
        
    }
    public static IEnumerator<GPoint> CircleLineIntersection(double cx, double cy, double r, double x1, double y1, double x2, double y2)
    {
        double m = (y2 - y1) / (x2 - x1);
        double b = y1 - m * x1;

        double A = 1 + m * m;
        double B = -2 * cx + 2 * m * b - 2 * cy * m;
        double C = cx * cx + b * b + cy * cy - 2 * b * cy - r * r;

        double discriminant = B * B - 4 * A * C;

        if (discriminant == 0)
        {
            double x = -B / (2 * A);
            double y = m * x + b;
            yield return new GPoint(new Point(x, y));
        }
        else if(discriminant > 0)
        {
            double ix1 = (-B + Math.Sqrt(discriminant)) / (2 * A);
            double iy1 = m * ix1 + b;
            double ix2 = (-B - Math.Sqrt(discriminant)) / (2 * A);
            double iy2 = m * ix2 + b;
            yield return new GPoint(new Point(ix1, iy1));
            yield return new GPoint(new Point(ix2, iy2));
        }
        
    }

    public static IEnumerator<MyExpression> CircleCircleIntersection(double x1, double y1, double r1, double x2, double y2, double r2)
    {
        // Distancia entre los centros de los círculos
        double d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        
        // Comprobar si los círculos se tocan o se cruzan
        if (d <= r1 + r2 && d >= Math.Abs(r1 - r2))
        {
            // Calcular puntos de intersección
            double a = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(d, 2)) / (2 * d);
            double centerX = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(a, 2));

            // Calcular las coordenadas de los puntos de intersección
            double x3 = x1 + a * (x2 - x1) / d;
            double y3 = y1 + a * (y2 - y1) / d;

            double ix1 = x3 + centerX * (y2 - y1) / d;
            double iy1 = y3 - centerX * (x2 - x1) / d;

            double ix2 = x3 - centerX * (y2 - y1) / d;
            double iy2 = y3 + centerX * (x2 - x1) / d;

            // Añadir los puntos de intersección
            yield return new GPoint(new Point(ix1, iy1));
            yield return new GPoint(new Point(ix2, iy2));
        }
    }
}