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
using System.ComponentModel;
using System.Windows.Interop;
namespace DiagramDesigner
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        //[System.Runtime.InteropServices.DllImport("uxtheme.dll")]
        //private static extern int SetWindowTheme(IntPtr hwnd, string appname, string idlist); 

        private string arguments = "";
        public Test()
        {
            InitializeComponent();
        }

        public Test( StartupEventArgs e)
        {
            InitializeComponent();
            arguments = e.Args[0];
            //IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            //SetWindowTheme(windowHandle, "", "");
        }

        void lstNodes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UIElement elem = (UIElement)lstNodes.InputHitTest(e.GetPosition(lstNodes));
            while (elem != lstNodes)
            {
                if (elem is ListBoxItem)
                {
                    object selectedItem = ((ListBoxItem)elem).Content;
                    MyDesigner.SelectNode(((TextBlock)((StackPanel)selectedItem).Children[2]).Text);
                    return;
                }
                elem = (UIElement)VisualTreeHelper.GetParent(elem);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            IBAyesSplash splash = new IBAyesSplash();
            splash.ShowDialog();
            if (arguments != "")
            {
                MyDesigner.OpenTest(arguments);
               
            }
        }
        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MyDesigner.Window_Closing(sender, e);
            
        }


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

        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {

            System.Console.WriteLine(sender.ToString());
        }

    }
}
