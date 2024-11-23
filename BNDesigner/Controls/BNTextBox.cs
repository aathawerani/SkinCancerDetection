using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DiagramDesigner.Controls
{
    public class BNTextBox: TextBox
    {
        //string _title;
        //public string Title
        //{
        //    get { return _title; }
        //    set { _title = value; }
        //}

        public string NodeTitle
        {
            get { return (string)GetValue(NodeTitleProperty); }
            set { SetValue(NodeTitleProperty, value); }
        }

        public static DependencyProperty NodeTitleProperty = DependencyProperty.Register("NodeTitle", typeof(string), typeof(BNTextBox));

        public string NodeStatus
        {
            get { return (string)GetValue(NodeStatusProperty); }
            set { SetValue(NodeStatusProperty, value); }
        }

        public static DependencyProperty NodeStatusProperty = DependencyProperty.Register("NodeStatus", typeof(string), typeof(BNTextBox));

        public string NodeImage
        {
            get { return (string)GetValue(NodeImageProperty); }
            set { SetValue(NodeImageProperty, value); }
        }

        public static DependencyProperty NodeImageProperty = DependencyProperty.Register("NodeImage", typeof(string), typeof(BNTextBox));

        public string NodeProbab
        {
            get { return (string)GetValue(NodeProbabProperty); }
            set { SetValue(NodeProbabProperty, value); }
        }

        public static DependencyProperty NodeProbabProperty = DependencyProperty.Register("NodeProbab", typeof(string), typeof(BNTextBox));

        public string NodeFalseProbab
        {
            get { return (string)GetValue(NodeFalseProbabProperty); }
            set { SetValue(NodeFalseProbabProperty, value); }
        }

        public static DependencyProperty NodeFalseProbabProperty = DependencyProperty.Register("NodeFalseProbab", typeof(string), typeof(BNTextBox));

        public string NodeColor
        {
            get { return (string)GetValue(NodeColorProperty); }
            set { SetValue(NodeColorProperty, value); }
        }
        public static DependencyProperty NodeColorProperty = DependencyProperty.Register("NodeColor", typeof(string), typeof(BNTextBox));


        public double TruePercent
        {
            get { return (double)GetValue(TruePercentProperty); }
            set { SetValue(TruePercentProperty, value); }
        }
        public static DependencyProperty TruePercentProperty = DependencyProperty.Register("TruePercent", typeof(double), typeof(BNTextBox));

        public double FalsePercent
        {
            get { return (double)GetValue(FalsePercentProperty); }
            set { SetValue(FalsePercentProperty, value); }
        }
        public static DependencyProperty FalsePercentProperty = DependencyProperty.Register("FalsePercent", typeof(double), typeof(BNTextBox));
    }
}
