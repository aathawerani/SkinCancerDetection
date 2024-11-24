using System;
using System.Collections;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
//using System.Windows.Markup;
using System.Windows.Media;
//using System.Xml;
//using System.Xml.Linq;
//using Microsoft.Win32;
using DiagramDesigner.Controls;
using IBAyes.Bayesian;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

namespace DiagramDesigner
{

    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class DesignerItem : ContentControl, ISelectable, IGroupable
    {

        //[System.Runtime.InteropServices.DllImport("uxtheme.dll")]
        //private static extern int SetWindowTheme(IntPtr hwnd, string appname, string idlist); 

        public static RoutedCommand SetEvidence = new RoutedCommand();
        public static RoutedCommand ClearEvidence = new RoutedCommand();
        public static RoutedCommand SetNodeType = new RoutedCommand();
        public static RoutedCommand SetNodeTypeNMax = new RoutedCommand();
        public static RoutedCommand SetNodeTypeCAST = new RoutedCommand();
        public static RoutedCommand ResizeToFitText = new RoutedCommand();


        private Point? dragStartPoint = null;

        #region ID
        private Guid id;
        public Guid ID
        {
            get { return id; }
        }

        Connector sourceConnector;
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
            this.CommandBindings.Add(new CommandBinding(SetNodeType, SetNodeType_Executed));
            this.CommandBindings.Add(new CommandBinding(SetNodeTypeNMax, SetNodeTypeNMax_Executed));
            this.CommandBindings.Add(new CommandBinding(SetNodeTypeCAST, SetNodeTypeCAST_Executed));
            this.CommandBindings.Add(new CommandBinding(ResizeToFitText, ResizeToFitText_Executed));

            DesignerCanvas designer = this.Parent as DesignerCanvas;
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

        protected TextDecorationCollection GetUnderlineDecorator()
        {
            TextDecoration myUnderline = new TextDecoration();

            // Create a linear gradient pen for the text decoration.
            Pen myPen = new Pen();

            myPen.Brush = new LinearGradientBrush(Colors.Black, Colors.Black, new Point(0, 1.5), new Point(1, 1.5));
            //myPen.Brush.Opacity = 1.5;
            myPen.Thickness = 3;
            myPen.DashStyle = DashStyles.Solid;
            myUnderline.Pen = myPen;
            myUnderline.PenThicknessUnit = TextDecorationUnit.FontRecommended;

            // Set the underline decoration to a TextDecorationCollection and add it to the text block.
            TextDecorationCollection myCollection = new TextDecorationCollection();
            myCollection.Add(myUnderline);
            return myCollection;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            try
            {
                //ContentPresenter contentPresenters = this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                //ProgressBar pbar1 = (ProgressBar)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(contentPresenters, 0), 0), 0), 1), 1);
                //ProgressBar pbar2 = (ProgressBar)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(contentPresenters, 0), 0), 0), 2), 1);

                //IntPtr windowHandle = //new WindowInteropHelper(pbar1).Handle;
                //SetWindowTheme(windowHandle, "", "");

                //windowHandle = new WindowInteropHelper(pbar2).Handle;
                //SetWindowTheme(windowHandle, "", "");

                   
                base.OnMouseDoubleClick(e);
                bool eventProcessed = false;

                if (e.OriginalSource.GetType() == typeof(TextBlock))
                {
                    BNTextBox node = ((BNTextBox)Content);
                    TextBlock thisText = ((TextBlock)e.OriginalSource);

                    if ((thisText.Text.Contains("%")) & (thisText.Text.StartsWith(BNNode.States[0])))
                    {
                        if (BNNode.EvidenceOn == 0)
                        {
                            BNNode.ClearEvidence();
                            node.NodeProbab = BNNode.States[0] + " ?? %";
                            node.TruePercent = 0;
                            thisText.TextDecorations.RemoveAt(0);
                        }
                        else
                        {
                            if (BNNode.EvidenceOn == 1)
                            {
                                //otherText.TextDecorations.RemoveAt(0);
                                ClearUnderlineProperty(1);
                            }
                            BNNode.SetEvidence(0);
                            node.NodeProbab = BNNode.States[0] + " 100%";
                            node.TruePercent = 100;
                            ((TextBlock)e.OriginalSource).TextDecorations.Add(GetUnderlineDecorator());
                                                       
                            node.NodeFalseProbab = BNNode.States[1] + " 0%";
                            node.FalsePercent = 0;

                        }
                        eventProcessed = true;
                    }

                    else if ((((TextBlock)e.OriginalSource).Text.Contains("%")) & (((TextBlock)e.OriginalSource).Text.StartsWith(BNNode.States[1])))
                    {
                        if (BNNode.EvidenceOn == 1)
                        {
                            BNNode.ClearEvidence();
                            node.NodeFalseProbab = BNNode.States[1] + " ?? %";
                            node.FalsePercent = 0;
                            ((TextBlock)e.OriginalSource).TextDecorations.RemoveAt(0);
                        }
                        else
                        {
                            if (BNNode.EvidenceOn == 0)
                            {
                                ClearUnderlineProperty(0);
                            }
                            BNNode.SetEvidence(1);
                            node.NodeFalseProbab = BNNode.States[1] + " 100%";
                            node.FalsePercent = 100;
                            ((TextBlock)e.OriginalSource).TextDecorations.Add(GetUnderlineDecorator());

                            node.NodeProbab = BNNode.States[0] + " 0%";
                            node.TruePercent = 0;
                        }
                        eventProcessed = true;
                    }
                }
                if (!eventProcessed)
                {

                    ContentPresenter contentPresenter =
                        this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;

                    frmNodeProperties frmNodeProp = new frmNodeProperties();
                    frmNodeProp.ShowNodePropertiesDialog(BNNode);

                    if (frmNodeProp.NodeNameChanged)
                    {
                        //Need to adjust node position in left pane if node name is changed
                        //Unable to find a quick and clean way :(

                        //Get old index of node in left pane listbox
                        StackPanel nodeItem;
                        ListBox lstExplorer = ((Test)GetMainWindow()).lstNodes;
                        int oldIndex = 0;
                        while (oldIndex < lstExplorer.Items.Count)
                        {
                            nodeItem = (StackPanel)lstExplorer.Items[oldIndex];
                            if (((TextBlock)nodeItem.Children[1]).Text.CompareTo(BNNode.NodeID) == 0)
                            {
                                ((TextBlock)nodeItem.Children[2]).Text = BNNode.Name;
                                break;
                            }
                            oldIndex++;
                        }

                        StackPanel item = (StackPanel)(lstExplorer.Items[oldIndex]);

                        //Now get new index where this node should be inserted in alphabetical order

                        int newIndex;
                        newIndex = 0;
                        while (newIndex < lstExplorer.Items.Count)
                        {
                            if (newIndex != oldIndex)
                            {
                                nodeItem = (StackPanel)lstExplorer.Items[newIndex];
                                if (((TextBlock)nodeItem.Children[2]).Text.CompareTo(BNNode.Name) > 0)
                                    break;
                            }
                            newIndex++;
                        }

                        if (oldIndex != newIndex)
                        {
                            lstExplorer.Items.RemoveAt(oldIndex);
                            if (oldIndex < newIndex)
                                newIndex--;

                            lstExplorer.Items.Insert(newIndex, item);
                        }
                    }
                    UpdateVisuals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IBAyes Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            ContextMenu nodeMenu = new ContextMenu();
            //ContextMenu nodeMenu = ((DesignerItem)e.Source).ContextMenu;
            MenuItem mn1 = new MenuItem();
            mn1.Header = "Set Evidence";
            for (int i = 0; i < BNNode.States.Count; i++)
            {
                MenuItem submenu = new MenuItem();
                submenu.Header = BNNode.States[i];
                submenu.Command = SetEvidence;
                submenu.CommandParameter = i.ToString();
                if (i == BNNode.EvidenceOn)
                {
                    submenu.Header = submenu.Header;
                    submenu.FontWeight = FontWeights.Bold;
                }
                mn1.Items.Add(submenu);

            }
            nodeMenu.Items.Add(mn1);

            //Adding Node Type Menu
            MenuItem mn2 = new MenuItem();
            mn2.Header = "Clear Evidence";
            mn2.Command = ClearEvidence;
            nodeMenu.Items.Add(mn2);

            MenuItem mn3 = new MenuItem();
            mn3.Header = "Node Type";
            MenuItem submenu1 = new MenuItem();
            submenu1.Header = "General";
            submenu1.Command = SetNodeType;
            submenu1.CommandParameter = enmNodeType.General;
            if (BNNode.NodeType == enmNodeType.General)
                submenu1.FontWeight = FontWeights.Bold;
            mn3.Items.Add(submenu1);

            MenuItem submenu2 = new MenuItem();
            submenu2.Header = "NoisyOR";
            submenu2.Command = SetNodeType;
            submenu2.CommandParameter = enmNodeType.NoisyOR;
            if (BNNode.NodeType == enmNodeType.NoisyOR)
                submenu2.FontWeight = FontWeights.Bold;
            mn3.Items.Add(submenu2);

            MenuItem submenu4 = new MenuItem();
            submenu4.Header = "CAST";
            submenu4.Command = SetNodeType;
            submenu4.CommandParameter = enmNodeType.CAST;
            if (BNNode.NodeType == enmNodeType.CAST)
                submenu4.FontWeight = FontWeights.Bold;
            mn3.Items.Add(submenu4);

            nodeMenu.Items.Add(mn3);

            //Adding Resize to fit text menu
            MenuItem mn4 = new MenuItem();
            mn4.Header = "Resize to fit Text";
            mn4.Command = ResizeToFitText;
            nodeMenu.Items.Add(mn4);

            this.ContextMenu = nodeMenu;
        }
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);

        }
        void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    if (VisualTreeHelper.GetChildrenCount(contentPresenter) > 0)
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
            if (BNNode.EvidenceOn > -1)
                SetUnderlineProperty(BNNode.EvidenceOn);
            }

        }

        #region Set Evidence Command
        private void SetEvidence_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            int prevEvidence = ((DesignerItem)sender).BNNode.EvidenceOn;

            ((DesignerItem)sender).BNNode.SetEvidence(Convert.ToInt32(e.Parameter));

            int newEvidence = ((DesignerItem)sender).BNNode.EvidenceOn;

            if (prevEvidence != newEvidence)
            {
                if (prevEvidence >=0)
                    ((DesignerItem)sender).ClearUnderlineProperty(prevEvidence);

                if (newEvidence >=0)
                    ((DesignerItem)sender).SetUnderlineProperty(newEvidence);
            }
  
        }

        #endregion

        #region Change Node Type
        private void SetNodeType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ((DesignerItem)sender).BNNode.ChangeNodeType((enmNodeType)e.Parameter);
                SetNodeTypeImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IBAYES Error");
            }
        }

        private void SetNodeTypeNMax_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((DesignerItem)sender).BNNode.ChangeNodeType(enmNodeType.NoisyMax);
            SetNodeTypeImage();
        }


        private void SetNodeTypeCAST_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((DesignerItem)sender).BNNode.ChangeNodeType(enmNodeType.CAST);
            SetNodeTypeImage();
        }

        public void UpdateVisuals()
        {

            BNTextBox node = ((BNTextBox)Content);
            node.NodeTitle = BNNode.Name;
            node.NodeProbab = BNNode.States[0] + " " + Convert.ToInt32((BNNode.GetPosteriorProbab(0) * 100)).ToString() + "%";
            node.NodeFalseProbab = BNNode.States[1] + " " + Convert.ToInt32(BNNode.GetPosteriorProbab(1) * 100).ToString() + "%";
            node.TruePercent = BNNode.GetPosteriorProbab(0) * 100;
            node.FalsePercent = BNNode.GetPosteriorProbab(1) * 100;
            //if (BNNode.EvidenceOn >= 0)
            //    node.NodeStatus = "Resources/Images/info.ico";
            //else
            //    node.NodeStatus = "";
            SetNodeTypeImage();
        }

        public void SetNodeTypeImage()
        {
            BNTextBox node = ((BNTextBox)this.Content);

            if (BNNode.NodeType == enmNodeType.General)
            {
                node.NodeImage = "(G)";
                //node.NodeImage = "Resources/Images/NodeTypeGeneral.bmp";
            }
            if ((BNNode.NodeType == enmNodeType.NoisyMax) ^ (BNNode.NodeType == enmNodeType.NoisyOR))
            {
                node.NodeImage = " (O)";
                //node.NodeImage = "Resources/Images/NodeTypeNOR.bmp"; //"/DiagramDesigner;component/Resources/Images/NodeTypeNOR.bmp";
            }
            if (BNNode.NodeType == enmNodeType.CAST)
            {
                node.NodeImage = " (C)";
                //node.NodeImage = "Resources/Images/NodeTypeCAST.bmp";  //"/DiagramDesigner;component/Resources/Images/NodeTypeCAST.bmp";
            }

        }


        #endregion

        #region Clear Evidence Command
        public void ClearEvidence_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClearNodeEvidence();
        }

        public void ClearNodeEvidence()
        {
            int evidenceOn = BNNode.EvidenceOn;

            if (evidenceOn >= 0)
            {
                BNNode.ClearEvidence();
                ClearUnderlineProperty(evidenceOn);
            }
        }

        #endregion

        #region Resize to fit text Command
        private void ResizeToFitText_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable<DesignerItem> selectedDesignerItems =
                ((DesignerCanvas)this.Parent).SelectionService.CurrentSelection.OfType<DesignerItem>();

            BNTextBox obj;
            foreach (DesignerItem item in selectedDesignerItems)
            {
                obj = (BNTextBox)item.Content;

                //120 is minimum node size, 15 characters fit in this size so we need to adjust size when it exceeds 15
                if (obj.NodeTitle.Length > 15)
                {
                    item.Width = 120 + ((obj.NodeTitle.Length - 15) * 6);
                }

            }
        }
        #endregion



        private Window GetMainWindow()
        {
            //Get parent window reference so that    
            DependencyObject element = this;
            while (element != null && !(element is Window))
                element = VisualTreeHelper.GetParent(element);

            return element as Window;
        }

        private void AddNodetoExplorer(string nodeName)
        {
            //Add node name to node list
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;

            Image img = new Image();
            Uri uri = new Uri("Resources/images/NodeTypeGeneral.bmp", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;

            TextBlock txt = new TextBlock();
            txt.Text = nodeName;
            //txt.Padding.Left = 2;

            pnl.Children.Add(img);
            pnl.Children.Add(new Label());
            pnl.Children.Add(txt);

            ((Test)GetMainWindow()).lstNodes.Items.Add(pnl);
        }

        private TextBlock GetProbTextBlock(int stateIndex)
        {
            ContentPresenter contentPresenter = this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
            Label probLabel;
            int childIndex=-1; 

            if (stateIndex ==0)
                childIndex = 1;
            else if (stateIndex ==1)
                childIndex = 2;

            if (VisualTreeHelper.GetChildrenCount(contentPresenter) < 0) return null;

            probLabel = (Label)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(contentPresenter, 0), 0), 0), childIndex), 0);

            TextBlock probTextBlock = (TextBlock)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(probLabel, 0), 0), 0);

            return probTextBlock;
        }

        public void SetUnderlineProperty(int stateIndex)
        {
            TextBlock tb = GetProbTextBlock(stateIndex);
            if (tb != null)
                tb.TextDecorations.Add(GetUnderlineDecorator());
        }

        protected void ClearUnderlineProperty(int stateIndex)
        {
            TextBlock tb = GetProbTextBlock(stateIndex);
            if (tb != null)
            {
                if (tb.TextDecorations.Count > 0)
                    tb.TextDecorations.RemoveAt(0);
            }
        }


    }
}
