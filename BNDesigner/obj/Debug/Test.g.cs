﻿#pragma checksum "..\..\Test.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1C56DCA55899F0458560533DD3D237DB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DiagramDesigner;
using DiagramDesigner.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DiagramDesigner {
    
    
    /// <summary>
    /// Test
    /// </summary>
    public partial class Test : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 45 "..\..\Test.xaml"
        internal System.Windows.Controls.Grid grdDesigner;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\Test.xaml"
        internal System.Windows.Controls.ListBox lstNodes;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\Test.xaml"
        internal DiagramDesigner.DesignerCanvas MyDesigner;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\Test.xaml"
        internal System.Windows.Controls.Slider zoomSlider;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/IBAyes;component/test.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Test.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\Test.xaml"
            ((DiagramDesigner.Test)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 9 "..\..\Test.xaml"
            ((DiagramDesigner.Test)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grdDesigner = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.lstNodes = ((System.Windows.Controls.ListBox)(target));
            
            #line 53 "..\..\Test.xaml"
            this.lstNodes.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.lstNodes_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MyDesigner = ((DiagramDesigner.DesignerCanvas)(target));
            return;
            case 5:
            this.zoomSlider = ((System.Windows.Controls.Slider)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
