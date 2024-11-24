using System;
using System.Collections;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using System.Windows.Markup;
using System.Windows.Media;
//using System.Xml;
//using System.Xml.Linq;
//using Microsoft.Win32;
using DiagramDesigner.Controls;
using DiagramDesigner.Bayesian;

namespace DiagramDesigner
{
    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class DesignerItem : ContentControl, ISelectable, IGroupable
    {

        public static RoutedCommand SetEvidence = new RoutedCommand();
        public static RoutedCommand ClearEvidence = new RoutedCommand();


        #region ID
        private Guid id;
        public Guid ID
        {
            get { return id; }
        }
        #endregion

        private Node node;
        public Node BNNode
        {
            get { return node; }
            set { node = value; }
        }


        #region ParentID
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(DesignerItem));
        #endregion

        #region Name
        public string Name
        {
            get { return GetValue(NameDP).ToString(); }
            set { SetValue(NameDP, value); }
        }
        public static readonly DependencyProperty NameDP = DependencyProperty.Register("Name", typeof(string), typeof(DesignerItem));
        #endregion

        #region IsGroup
        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }
        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(DesignerItem));
        #endregion

        #region IsSelected Property

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DesignerItem),
                                       new FrameworkPropertyMetadata(false));

        #endregion

        #region DragThumbTemplate Property

        // can be used to replace the default template for the DragThumb
        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
        }

        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        #endregion

        #region ConnectorDecoratorTemplate Property

        // can be used to replace the default template for the ConnectorDecorator
        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        #endregion

        #region IsDragConnectionOver

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectorDecorator is triggered
        // to be visible, see template
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }
        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                                         typeof(bool),
                                         typeof(DesignerItem),
                                         new FrameworkPropertyMetadata(false));

        #endregion

        static DesignerItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }

        public DesignerItem(Guid id)
        {
            this.id = id;
            this.Loaded += new RoutedEventHandler(DesignerItem_Loaded);
            this.CommandBindings.Add(new CommandBinding(SetEvidence, SetEvidence_Executed));
            this.CommandBindings.Add(new CommandBinding(ClearEvidence, ClearEvidence_Executed));

            //Temp code to Find node count
            DesignerCanvas designer = this.Parent as DesignerCanvas;//VisualTreeHelper.GetParent(this) as DesignerCanvas;
        //    IEnumerable<DesignerItem> designerItems = designer.Children.OfType<DesignerItem>();
            
        //    BNNode = new Node(designerItems.Count() +1, "Node " + (designerItems.Count()+1).ToString());
        }

        public DesignerItem()
            : this(Guid.NewGuid())
        {
        }


        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

            // update selection
            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                    if (this.IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                else if (!this.IsSelected)
                {
                    designer.SelectionService.SelectItem(this);
                }
                Focus();
            }

            e.Handled = false;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            frmNodeProperties frmNodeProp = new frmNodeProperties();
            frmNodeProp.ShowNodePropertiesDialog(BNNode);
        }


        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            ContextMenu nodeMenu = new ContextMenu();
            MenuItem mn1 = new MenuItem();
            mn1.Header = "Set Evidence";
            for (int i = 0; i<BNNode.States.Count;i++)
            {
                MenuItem submenu = new MenuItem();
                submenu.Header = BNNode.States[i];
                submenu.Command = SetEvidence;
                submenu.CommandParameter = i.ToString();
                if (i == BNNode.EvidenceOn)
                {
                    submenu.Header = submenu.Header + " - 100 %";
                }
                mn1.Items.Add(submenu);

            }
            nodeMenu.Items.Add(mn1);

            MenuItem mn2 = new MenuItem();
            mn2.Header = "Clear Evidence";
            mn2.Command = ClearEvidence;
            nodeMenu.Items.Add(mn2);

            this.ContextMenu = nodeMenu;

            //nodeMenu.Items.

            //this.ContextMenu.Items.Add("Hello World");
            //base.OnContextMenuOpening(
        }
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);
            //Console.WriteLine(this.ContextMenu.Items.Count);

        }
        void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    UIElement contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                    if (contentVisual != null)
                    {
                        DragThumb thumb = this.Template.FindName("PART_DragThumb", this) as DragThumb;
                        if (thumb != null)
                        {
                            ControlTemplate template =
                                DesignerItem.GetDragThumbTemplate(contentVisual) as ControlTemplate;
                            if (template != null)
                                thumb.Template = template;
                        }
                    }
                }
            }
        }

        #region Set Evidence Command
        private void SetEvidence_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((DesignerItem)sender).BNNode.SetEvidence(Convert.ToInt32(e.Parameter));
        }

        #endregion

        #region Clear Evidence Command
        private void ClearEvidence_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((DesignerItem)sender).BNNode.ClearEvidence();
        }

        #endregion

    }
}
