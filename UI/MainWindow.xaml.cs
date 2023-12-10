using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GeoWalle.Backend;
using GeoWalle.Backend.Model.Context;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;
using GeoWalle.Backend.Parser;
using Microsoft.Win32;

namespace GeoWalle.UI;

public partial class MainWindow
{
    private double zoom = 1;
    public MainWindow()
    {
        InitializeComponent();
            
    }
    private void DrawButtonClicked(object sender, RoutedEventArgs e)
    {
        Output.Text = "";
        string s = Input.Text;
        if (s == "")
            return;
        if (!Utils.IsBalanced(s)) throw new SyntaxException("Input no balanceado");
        Draw.canvas = MiCanvas;
        Draw.CanvasScrollViewer = CanvasScrollViewer;
        IEnumerable<Token> tokens = Lexer.GetTokens(s);
        Parser p = new Parser(tokens.GetEnumerator());
        try
        {
            IEnumerable<MyExpression> listExpressions = p.Parse(new Context());
            foreach (MyExpression expression in listExpressions)
            {
                if (expression!=null)
                {
                    Output.Text += expression.Evaluate()+"\n";
                }
            }
        }
        catch (MyException exception)
        {
                
            Output.Text = exception.Message +"\nPara mas info visite: "+exception.HelpLink;
        }
            
    }

    private void CleanCanvas(object sender, RoutedEventArgs e)
    {
        MiCanvas.Children.Clear();
    }

    private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
    {
        CanvasScrollViewer.ScrollToHorizontalOffset(CanvasScrollViewer.ScrollableWidth/2);
        CanvasScrollViewer.ScrollToVerticalOffset(CanvasScrollViewer.ScrollableHeight/2);
    }

    private void ImportClicked(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Archivos GEO (*.geo)|*.geo"; // Filtra solo archivos .geo
        if (openFileDialog.ShowDialog() == true)
        {
            Input.Text += File.ReadAllText(openFileDialog.FileName); 
        }
    }

    private void SaveCode(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Archivos GEO (*.geo)|*.geo";
        if (saveFileDialog.ShowDialog() == true)
        {
            File.WriteAllText(saveFileDialog.FileName, Input.Text);
        }
    }

    private void Zoom(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            if (e.Delta > 0)
            {
                
                zoom *= 1.1; 
                
            }
            else if (e.Delta < 0)
            {

                zoom /= 1.1;

            }

            MiCanvas.LayoutTransform = new ScaleTransform(zoom, zoom, CanvasScrollViewer.ActualWidth/2, CanvasScrollViewer.ActualHeight/2);
            e.Handled = true;
        }
    }
}