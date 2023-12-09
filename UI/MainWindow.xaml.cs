using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

using System.Windows.Shapes;
using GeoWalle.Backend;
using GeoWalle.Backend.Model.Context;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;

namespace GeoWalle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DrawPoint();
        }

        private void DrawPoint()
        {
            Ellipse circunferencia = new Ellipse
            {
                Width = 65,
                Height = 65,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(circunferencia, 428);
            Canvas.SetTop(circunferencia, 133);
            MiCanvas.Children.Add(circunferencia);
        }

        private void DrawButtonClicked(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            string s = Input.Text;
            if (!Utils.IsBalanced(s)) throw new SyntaxException("Input no balanceado");
            Draw.canvas = MiCanvas;
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
    }
}