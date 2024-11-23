using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiagramDesigner
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }
        //private void Test_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ClickCount == 2)
        //    {
        //        System.Console.WriteLine("Double Clicked");
        //    }
        //}


    }

    public partial class TestHandler
    {
        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                System.Console.WriteLine("Double Clicked");
            }
        }

        protected void Text_LostFocus(object sender, EventArgs e)
        {

            try
            {
                Console.WriteLine("Here");
            }
            catch (Exception ex)
            { throw ex; }
        }


        protected void Text_Changed(object sender, EventArgs e)
        {

            try
            {
                Console.WriteLine("Text_Changed");
            
            }
            catch (Exception ex)
            { throw ex; }
        }

    }
}
