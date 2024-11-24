using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using IBAyes.Bayesian;
using DiagramDesigner.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;


namespace DiagramDesigner
{
    public partial class DesignerCanvas : Canvas
    {
        private Network bnNetwork;
        private Point? rubberbandSelectionStartPoint = null;

        private SelectionService selectionService;

        internal SelectionService SelectionService
        {
            get
            {
                if (selectionService == null)
                    selectionService = new SelectionService(this);

                return selectionService;
            }
        }

        public Network BNNetwork { get { return bnNetwork; } }

       
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                // in case that this click is the start of a 
                // drag operation we cache the start point
                this.rubberbandSelectionStartPoint = new Point?(e.GetPosition(this));

                // if you click directly on the canvas all 
                // selected items are 'de-selected'
                SelectionService.ClearSelection();
                Focus();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
                this.rubberbandSelectionStartPoint = null;

            // ... but if mouse button is pressed and start
            // point value is set we do have one
            if (this.rubberbandSelectionStartPoint.HasValue)
            {
                // create rubberband adorner
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }
            }
            e.Handled = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null && !String.IsNullOrEmpty(dragObject.Xaml))
            {
                DesignerItem newItem = null;
                Object content = XamlReader.Load(XmlReader.Create(new StringReader(dragObject.Xaml)));

                if (content != null)
                {
                    newItem = new DesignerItem();
                    newItem.Content = content;

                    Point position = e.GetPosition(this);

                    if (dragObject.DesiredSize.HasValue)
                    {
                        Size desiredSize = dragObject.DesiredSize.Value;
                        newItem.Width = desiredSize.Width;
                        newItem.Height = desiredSize.Height;

                        DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                        DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));
                    }
                    else
                    {
                        DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X));
                        DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y));
                    }

                    newItem.Width = newItem.Width* 1.5;
                    newItem.Height = newItem.Height * 1.15;

                    Canvas.SetZIndex(newItem, this.Children.Count);
                    this.Children.Add(newItem);                    
                    SetConnectorDecoratorTemplate(newItem);

                    //update selection
                    this.SelectionService.SelectItem(newItem);
                    newItem.Focus();

                    ////ContentPresenter contentPresenter = newItem.Template.FindName("PART_ContentPresenter",newItem) as ContentPresenter;
                    //////ProgressBar pbar1 = (ProgressBar)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(contentPresenter, 0), 0), 0), 1), 1);
                    //ProgressBar pbar2 = (ProgressBar)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(contentPresenter, 0), 0), 0), 2), 1);
                        

                    if (((BNTextBox)content).NodeTitle.Trim() == "G")
                        newItem.BNNode = bnNetwork.CreateNode(enmNodeType.General); 
                    else if (((BNTextBox)content).NodeTitle.Trim() == "N")
                        newItem.BNNode = bnNetwork.CreateNode(enmNodeType.NoisyOR);
                    else if (((BNTextBox)content).NodeTitle.Trim() == "C")
                        newItem.BNNode = bnNetwork.CreateNode(enmNodeType.CAST);

                    newItem.UpdateVisuals();

                    //Add node name to node list
                    AddNodetoExplorer(newItem.BNNode.Name,newItem.BNNode.NodeID);
                }

                e.Handled = true;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        private void SetConnectorDecoratorTemplate(DesignerItem item)
        {
            if (item.ApplyTemplate() && item.Content is UIElement)
            {
                ControlTemplate template = DesignerItem.GetConnectorDecoratorTemplate(item.Content as UIElement);
                Control decorator = item.Template.FindName("PART_ConnectorDecorator", item) as Control;
                if (decorator != null && template != null)
                    decorator.Template = template;
            }
        }

        private void UpdateNodeAppearance()
        {
              IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
              foreach (DesignerItem item in designerItems)
              {
                  item.UpdateVisuals();
              }

              IEnumerable<Connection> connections = this.Children.OfType<Connection>();
              foreach (Connection conn in connections)
              {
                  conn.UpdateVisuals();
              }
        }

        private Window GetMainWindow()
        {
            //Get parent window reference so that    
            DependencyObject element = this;
            while (element != null && !(element is Window))
                element = VisualTreeHelper.GetParent(element);

            return element as Window;
        }

        private void AddNodetoExplorer(string nodeName,string nodeID)
        {
            //Add node name to node list
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;

            Image img = new Image();
            Uri uri = new Uri("Resources/images/NodeTypeGeneral.bmp", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;

            TextBlock txtnodeID = new TextBlock();
            txtnodeID.Text = nodeID;
            txtnodeID.Width = 5;
            txtnodeID.Visibility = Visibility.Hidden;    

            TextBlock txt = new TextBlock();
            txt.Text = nodeName;
            //txt.Padding.Left = 2;

            pnl.Children.Add(img);
            pnl.Children.Add(txtnodeID);
            pnl.Children.Add(txt);

            int i=0;
            ListBox lstExplorer = ((Test)GetMainWindow()).lstNodes;
            while (i < lstExplorer.Items.Count)
            {
                StackPanel nodeItem = (StackPanel)lstExplorer.Items[i];
                if (((TextBlock)nodeItem.Children[2]).Text.CompareTo(nodeName) > 0)
                    break;
                i++;
            }
            lstExplorer.Items.Insert(i,pnl);
        }
    }
}
