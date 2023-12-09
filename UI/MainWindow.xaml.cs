using System.Collections.Generic;
using System.Windows;
using GeoWalle.Backend;
using GeoWalle.Backend.Model.Context;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;
using GeoWalle.Backend.Parser;

namespace GeoWalle.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void DrawButtonClicked(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            string s = Input.Text;
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
    }
}