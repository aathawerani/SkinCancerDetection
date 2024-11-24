using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DiagramDesigner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        { }
        public void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length>0)
                new Test(e).Show();
            else
                new Test().Show();
            
        } 

    }   
}
