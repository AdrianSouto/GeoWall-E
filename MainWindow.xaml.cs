using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

using System.Windows.Shapes;
using GeoWalle.Backend;
using GeoWalle.Backend.Model.Context;
using GeoWalle.Backend.Model.Expressions;
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

        /*private void DrawPoint()
        {
            Random random = new Random();
            double x = random.NextDouble() * MiCanvas.ActualWidth;
            double y = random.NextDouble() * MiCanvas.ActualHeight;
            
            Ellipse punto = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = Brushes.Black
            };
            Canvas.SetLeft(punto, x);
            Canvas.SetLeft(punto, y);
            MiCanvas.Children.Add(punto);
        }*/

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            string s = Input.Text;
            if (!Utils.IsBalanced(s)) throw new SyntaxException("Input no balanceado");
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
            catch (Exception exception)
            {
                
                Output.Text = exception.Message +"\nPara mas info visite: "+exception.HelpLink;
            }
            
        }
    }
}