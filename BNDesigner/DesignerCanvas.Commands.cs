using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using IBAyes.Bayesian;
using DiagramDesigner.Controls;
using System.ComponentModel;


//using System;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Media;
//using DiagramDesigner.Controls;
//using System.Collections.Generic;

namespace DiagramDesigner
{
    public partial class DesignerCanvas
    {
        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        public static RoutedCommand DistributeVertical = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();
        public static RoutedCommand UpdateBelief = new RoutedCommand();
        public static RoutedCommand ClearAllEvidences= new RoutedCommand();
        public static RoutedCommand ChangeNetworkProperties = new RoutedCommand();
        public static RoutedCommand ToggleExplorer = new RoutedCommand();
        public static RoutedCommand Export = new RoutedCommand();
        public static RoutedCommand PerformSA= new RoutedCommand();
        public static RoutedCommand Help = new RoutedCommand();
        public static RoutedCommand About = new RoutedCommand();
     //   public static RoutedCommand Close = new RoutedCommand();


        public DesignerCanvas()
        {
            bnNetwork = new IBAyes.Bayesian.Network();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Group, Group_Executed, Group_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Ungroup, Ungroup_Executed, Ungroup_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringForward, BringForward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringToFront, BringToFront_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendBackward, SendBackward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendToBack, SendToBack_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignTop, AlignTop_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignBottom, AlignBottom_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignLeft, AlignLeft_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignRight, AlignRight_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeHorizontal, DistributeHorizontal_Executed, Distribute_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeVertical, DistributeVertical_Executed, Distribute_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SelectAll, SelectAll_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.UpdateBelief, UpdateBelief_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.ClearAllEvidences, ClearAllEvidences_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.ChangeNetworkProperties, ChangeNetworkProperties_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.ToggleExplorer, ToggleExplorer_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Export, Export_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.PerformSA, PerformSA_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Help, Help_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.About, About_Executed));
          //  this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Close, Window_Closing_Executed));

            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            this.AllowDrop = true;
            
            Clipboard.Clear();
            //OpenTest(@"C:\Users\alarik\Desktop\testing.ibs");
        }
         public void Window_Closing_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to save current document?", "IBAyes - Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
            
               // Save_Executed(sender,e.RoutedEvent);
            }
            
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to save current document?", "IBAyes - Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
                IEnumerable<Connection> connections = this.Children.OfType<Connection>();

                XElement designerItemsXML = SerializeDesignerItems(designerItems);
                XElement connectionsXML = SerializeConnections(connections);

                XElement root = new XElement("Root");
                root.Add(designerItemsXML);
                root.Add(connectionsXML);

                SaveFile(root);
         
            }
            
        }

        #region New Command

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to save current document?", "IBAyes - Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Save_Executed(sender, e);
            }
            bnNetwork = new IBAyes.Bayesian.Network();
            ((Test)GetMainWindow()).lstNodes.Items.Clear();

            this.Children.Clear();
            this.SelectionService.ClearSelection();
        }

        #endregion

        public void OpenTest(string args)
        {
            ((Test)GetMainWindow()).Title = "IBAyes - " +  args.Substring(args.LastIndexOf(@"\")+1);
            XElement root = XElement.Load(args);
            if (root == null)
                return;

            bnNetwork = new IBAyes.Bayesian.Network();
            this.Children.Clear();
            this.SelectionService.ClearSelection();

            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            foreach (XElement itemXML in itemsXML)
            {
                Guid id = new Guid(itemXML.Element("ID").Value);
                DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0, false);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }

            this.InvalidateVisual();

            foreach (DesignerItem item in this.Children)
            {
                if (item.BNNode.EvidenceOn > -1 )
                    item.SetUnderlineProperty(item.BNNode.EvidenceOn);
            }

            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

                Connection connection = new Connection(sourceConnector, sinkConnector);
                Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));

                connection.BNConnection = bnNetwork.CreateConnection2(bnNetwork.GetNodeObjectById(connectionXML.Element("SourceNodeID").Value), bnNetwork.GetNodeObjectById(connectionXML.Element("SinkNodeID").Value));
                this.Children.Add(connection);
            }

           ((Test)GetMainWindow()).lstNodes.Items.Clear();
            foreach (Node node in bnNetwork.Nodes)
            {
                AddNodetoExplorer(node.Name, node.NodeID);
            }
            
        }

        #region Open Command

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to save current document?","IBAyes - Confirmation",MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes)
            {
               Save_Executed(sender, e);
            }
            XElement root = LoadSerializedDataFromFile();

            if (root == null)
                return;

            bnNetwork = new IBAyes.Bayesian.Network();
            this.Children.Clear();
            this.SelectionService.ClearSelection();

            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            foreach (XElement itemXML in itemsXML)
            {
                Guid id = new Guid(itemXML.Element("ID").Value);
                DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0,false);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }

            this.InvalidateVisual();

            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

                Connection connection = new Connection(sourceConnector, sinkConnector);
                Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));

                connection.BNConnection = bnNetwork.CreateConnection2(bnNetwork.GetNodeObjectById(connectionXML.Element("SourceNodeID").Value), bnNetwork.GetNodeObjectById(connectionXML.Element("SinkNodeID").Value));
                this.Children.Add(connection);
            }

            ((Test)GetMainWindow()).lstNodes.Items.Clear();
            foreach (Node node in bnNetwork.Nodes)
            {
                AddNodetoExplorer(node.Name,node.NodeID);
            }

            //foreach (DesignerItem item in this.Children)
            //{
            //    if (item.BNNode.EvidenceOn > -1)
            //        item.SetUnderlineProperty(item.BNNode.EvidenceOn);
            //}
        }

        #endregion

        #region Save Command

        public void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            XElement designerItemsXML = SerializeDesignerItems(designerItems);
            XElement connectionsXML = SerializeConnections(connections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);
            SaveFile(root);
        }

        #endregion
        

        #region Print Command

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.ClearSelection();

            PrintDialog printDialog = new PrintDialog();

            if (true == printDialog.ShowDialog())
            {
                printDialog.PrintVisual(this, "Bayesian Network");

            }
        }

        

        //public static void ShowPrintPreview(FixedDocument fixedDoc)
        //{

        //    // create a document
        //    FixedDocument document = new FixedDocument();
        //    document.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

        //    Window wnd = new Window();

        //    DocumentViewer viewer = new DocumentViewer();

        //    viewer.Document = fixedDoc;

        //    wnd.Content = viewer;

        //    wnd.ShowDialog();

        //}

        public void ExportToPng(string path, Canvas surface)
        {
            if (path == null) return;

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(surface.ActualWidth, surface.ActualHeight);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }

            // Restore previously saved layout
            surface.LayoutTransform = transform;
        }

        #endregion

        #region Copy Command

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Paste Command

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement root = LoadSerializedDataFromClipBoard();

            if (root == null)
                return;

            // create mapping of ID's dictionary

            Dictionary<Guid, Guid> mappingOldToNewIDs = new Dictionary<Guid, Guid>();
            Dictionary<string, string> mappingOldToNewNodeIDs = new Dictionary<string, string>();

            List<ISelectable> newItems = new List<ISelectable>();
            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");

            double offsetX = Double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
            double offsetY = Double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);

            //creating Designer Item
            foreach (XElement itemXML in itemsXML)
            {
                Guid oldID = new Guid(itemXML.Element("ID").Value);
                Guid newID = Guid.NewGuid();
                mappingOldToNewIDs.Add(oldID, newID);
                DesignerItem item = DeserializeDesignerItem(itemXML, newID, offsetX, offsetY,true);
                item.UpdateVisuals();
                mappingOldToNewNodeIDs.Add(itemXML.Element("BNNode").Element("Node").Element("NodeID").Value, item.BNNode.NodeID);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
                newItems.Add(item);
            }


            // update group hierarchy
            SelectionService.ClearSelection();
            foreach (DesignerItem el in newItems)
            {
                if (el.ParentID != Guid.Empty)
                    el.ParentID = mappingOldToNewIDs[el.ParentID];
                
            }


            foreach (DesignerItem item in newItems)
            {
                if (item.ParentID == Guid.Empty)
                {
                    SelectionService.AddToSelection(item);
                }
            }

            // create Connections
            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid oldSourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid oldSinkID = new Guid(connectionXML.Element("SinkID").Value);

                string oldSourceNodeID = connectionXML.Element("SourceNodeID").Value;
                string oldSinkNodeID = connectionXML.Element("SinkNodeID").Value;

                // if the node being copied does not have any parents

                if ((mappingOldToNewIDs.ContainsKey(oldSourceID) && mappingOldToNewIDs.ContainsKey(oldSinkID)))
                {
                    Guid newSourceID = mappingOldToNewIDs[oldSourceID];
                    Guid newSinkID = mappingOldToNewIDs[oldSinkID];

                    string newSourceNodeID = mappingOldToNewNodeIDs[oldSourceNodeID];
                    string newSinkNodeID = mappingOldToNewNodeIDs[oldSinkNodeID];

                    String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                    String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                    Connector sourceConnector = GetConnector(newSourceID, sourceConnectorName);
                    Connector sinkConnector = GetConnector(newSinkID, sinkConnectorName);

                    Connection connection = new Connection(sourceConnector, sinkConnector);
                    Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                    connection.BNConnection = bnNetwork.CreateConnection2(bnNetwork.GetNodeObjectById(newSourceNodeID), bnNetwork.GetNodeObjectById(newSinkNodeID));

                    this.Children.Add(connection);

                    SelectionService.AddToSelection(connection);
                }

                    //if the node being copied contains parent nodes
                else {
                    if (oldSourceNodeID != null && mappingOldToNewIDs.ContainsKey(oldSinkID))
                    {
                        Guid newSourceID = oldSourceID;
                        Guid newSinkID = mappingOldToNewIDs[oldSinkID];

                        string newSourceNodeID = oldSourceNodeID;
                        string newSinkNodeID = mappingOldToNewNodeIDs[oldSinkNodeID];
                        String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                        String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                        Connector sourceConnector = GetConnector(newSourceID, sourceConnectorName);
                        Connector sinkConnector = GetConnector(newSinkID, sinkConnectorName);

                        Connection connection = new Connection(sourceConnector, sinkConnector);
                        Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                        connection.BNConnection = bnNetwork.CreateConnection2(bnNetwork.GetNodeObjectById(newSourceNodeID), bnNetwork.GetNodeObjectById(newSinkNodeID));

                        this.Children.Add(connection);

                        SelectionService.AddToSelection(connection);
              

                    
                    }
                 
                }

               
            }

             

            ((Test)GetMainWindow()).lstNodes.Items.Clear();
            foreach (Node node in bnNetwork.Nodes)
            {
                AddNodetoExplorer(node.Name,node.NodeID);
            }
           
            DesignerCanvas.BringToFront.Execute(null, this);

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
            
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        #endregion

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCurrentSelection();
        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Cut Command

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
            DeleteCurrentSelection();
        }

        private void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion==

        #region Group Command

        private void Group_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var items = from item in this.SelectionService.CurrentSelection.OfType<DesignerItem>()
                        where item.ParentID == Guid.Empty
                        select item;

            Rect rect = GetBoundingRectangle(items);

            DesignerItem groupItem = new DesignerItem();
            groupItem.IsGroup = true;
            groupItem.Width = rect.Width;
            groupItem.Height = rect.Height;
            Canvas.SetLeft(groupItem, rect.Left);
            Canvas.SetTop(groupItem, rect.Top);
            Canvas groupCanvas = new Canvas();
            groupItem.Content = groupCanvas;
            Canvas.SetZIndex(groupItem, this.Children.Count);
            this.Children.Add(groupItem);

            foreach (DesignerItem item in items)
                item.ParentID = groupItem.ID;

            this.SelectionService.SelectItem(groupItem);
        }

        private void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentID == Guid.Empty
                         select item).Count();

            e.CanExecute = count > 1;
        }

        #endregion

        #region Ungroup Command

        private void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var groups = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          where item.IsGroup && item.ParentID == Guid.Empty
                          select item).ToArray();

            foreach (DesignerItem groupRoot in groups)
            {
                var children = from child in SelectionService.CurrentSelection.OfType<DesignerItem>()
                               where child.ParentID == groupRoot.ID
                               select child;

                foreach (DesignerItem child in children)
                    child.ParentID = Guid.Empty;

                this.SelectionService.RemoveFromSelection(groupRoot);
                this.Children.Remove(groupRoot);
                UpdateZIndex();
            }
        }

        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentID != Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 0;
        }

        #endregion

        #region BringForward Command

        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) descending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
            e.CanExecute = true;
        }

        #endregion

        #region BringToFront Command

        private void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();

            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, childrenSorted.Count - selectionSorted.Count + j++);
                }
                else
                {
                    Canvas.SetZIndex(item, i++);
                }
            }
        }

        #endregion

        #region SendBackward Command

        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) ascending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Max(i, currentIndex - 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region SendToBack Command

        private void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();
            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, j++);

                }
                else
                {
                    Canvas.SetZIndex(item, selectionSorted.Count + i++);
                }
            }
        }        

        #endregion

        #region AlignTop Command

        private void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = top - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        private void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region AlignVerticalCenters Command

        private void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignBottom Command

        private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignLeft Command

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = left - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignHorizontalCenters Command

        private void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double center = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = center - (Canvas.GetLeft(item) + item.Width / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignRight Command

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double right = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = right - (Canvas.GetLeft(item) + item.Width);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region DistributeHorizontal Command

        private void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemLeft = Canvas.GetLeft(item)
                                orderby itemLeft
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Double.MaxValue;
                double right = Double.MinValue;
                double sumWidth = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    left = Math.Min(left, Canvas.GetLeft(item));
                    right = Math.Max(right, Canvas.GetLeft(item) + item.Width);
                    sumWidth += item.Width;
                }

                double distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                double offset = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                    offset = offset + item.Width + distance;
                }
            }
        }

        private void Distribute_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region DistributeVertical Command

        private void DistributeVertical_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemTop = Canvas.GetTop(item)
                                orderby itemTop
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Double.MaxValue;
                double bottom = Double.MinValue;
                double sumHeight = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    top = Math.Min(top, Canvas.GetTop(item));
                    bottom = Math.Max(bottom, Canvas.GetTop(item) + item.Height);
                    sumHeight += item.Height;
                }

                double distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                double offset = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                    offset = offset + item.Height + distance;
                }
            }
        }

        #endregion

        #region SelectAll Command

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.SelectAll();
        }

        #endregion

        #region Bayesian Commands

        private void UpdateBelief_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                bnNetwork.UpdateBelief();
                UpdateNodeAppearance();
            }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void ClearAllEvidences_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();

                foreach (DesignerItem item in designerItems)
                {
                    if (item.BNNode.EvidenceOn >= 0)
                    {
                        item.ClearNodeEvidence();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void ChangeNetworkProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                frmNeworkProperties frm = new frmNeworkProperties(bnNetwork);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                                MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void ToggleExplorer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                double width;
                width = ((Test)GetMainWindow()).grdDesigner.ColumnDefinitions[0].Width.Value;

                if (width > 0)
                    ((Test)GetMainWindow()).grdDesigner.ColumnDefinitions[0].Width = new GridLength(0);
                else
                    ((Test)GetMainWindow()).grdDesigner.ColumnDefinitions[0].Width = new GridLength(150);
            }
            catch (Exception ex)
            {
                                MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Export_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.png)|*.png";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    ExportToPng(saveFile.FileName, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void PerformSA_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                IEnumerable<DesignerItem> selectedDesignerItems =
                    this.SelectionService.CurrentSelection.OfType<DesignerItem>();

                frmSensitivityAnalysis frm = new frmSensitivityAnalysis(bnNetwork);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                                MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string folderPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                folderPath = folderPath.Substring(0, folderPath.Length - 10);

                System.Diagnostics.Process.Start(folderPath+ "IBAyes.chm");
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                //AboutBox1 abtBox = new AboutBox1();
                //abtBox.ShowDialog();
                frmAbout abtBox = new frmAbout();
                abtBox.ShowDialog();
            }
            catch (Exception ex)
            {
                                MessageBox.Show(ex.Message,"IBAyes Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }


        #endregion


        #region Helper Methods

        
        private XElement LoadSerializedDataFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "IBAYES files (*.ibs)|*.ibs";
           
            if (openFile.ShowDialog() == true)
            {
                try
                {
                    ((Test)GetMainWindow()).Title = "IBAyes -" + openFile.SafeFileName; 
                    return XElement.Load(openFile.FileName);
                 }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        void SaveFile(XElement xElement)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "IBAYES files (*.ibs)|*.ibs";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;

                if (String.IsNullOrEmpty(clipboardData))
                    return null;
                try
                {
                    return XElement.Load(new StringReader(clipboardData));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        private XElement SerializeDesignerItems(IEnumerable<DesignerItem> designerItems)
        {

            XElement serializedItems1 = new XElement("DesignerItems",
                                     from item in designerItems
                                     let contentXaml = XamlWriter.Save(((DesignerItem)item).Content)
                                     let BNCPT = new XElement("CPT", new XElement("Rows", item.BNNode.CPT.Rows), new XElement("Columns", item.BNNode.CPT.Columns), new XElement("CPTTable", from c in item.BNNode.CPT.GetAllValues(item.BNNode.CPT.Rows, item.BNNode.CPT.Columns) select c))
                                     let BNStates = new XElement("State", item.BNNode.GetAllStates())
                                     let CASTPT = new XElement("CASTPT",new XElement("Rows",item.BNNode.CASTPT.Rows),new XElement("Columns",item.BNNode.CASTPT.Columns), new XElement("CASTPTTable", from c in item.BNNode.CASTPT.GetAllNoisyCASTValues(item.BNNode.CASTPT.Rows, item.BNNode.CASTPT.Columns, item.BNNode.NodeType) select c))
                                     let BNNode = new XElement("Node", new XElement("NodeID", item.BNNode.NodeID), new XElement("Name", item.BNNode.Name), new XElement("States", BNStates), new XElement("Parents", item.BNNode.GetAllParents()), new XElement("NodeType", item.BNNode.NodeType), BNCPT, new XElement("hasEvidence", item.BNNode.EvidenceOn), CASTPT, new XElement("Overriden", item.BNNode.Overriden))
                                     select new XElement("DesignerItem",
                                                new XElement("Left", Canvas.GetLeft(item)),
                                                new XElement("Top", Canvas.GetTop(item)),
                                                new XElement("Width", item.Width),
                                                new XElement("Height", item.Height),
                                                new XElement("ID", item.ID),
                                                new XElement("zIndex", Canvas.GetZIndex(item)),
                                                new XElement("IsGroup", item.IsGroup),
                                                new XElement("ParentID", item.ParentID),
                                                new XElement("BNNode", BNNode),
                                                new XElement("Content", contentXaml)
                                             )
                                          );


            return serializedItems1;
        }

        private XElement SerializeConnections(IEnumerable<Connection> connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", Canvas.GetZIndex(connection)),
                                      new XElement("ConnectionId", connection.BNConnection.ID),
                                      new XElement("SourceNodeID", connection.BNConnection.SourceNode.NodeID),
                                      new XElement("SinkNodeID", connection.BNConnection.SinkNode.NodeID)
                                     )
                                  );

            return serializedConnections;

        }

       
        public DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY,bool fromCopy)
        {
            DesignerItem item = new DesignerItem(id);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
            Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
            item.Content = content;

            //check if this has been called from Save or Copy and then creating Node

            if (fromCopy == false)
                item.BNNode = bnNetwork.CreateNode(itemXML.Element("BNNode").Element("Node").Element("NodeID").Value, itemXML.Element("BNNode").Element("Node").Element("Name").Value);
            else
                item.BNNode = bnNetwork.CreateNode();
        

            //To load CPT 
            int rows = Int32.Parse(itemXML.Element("BNNode").Element("Node").Element("CPT").Element("Rows").Value);
            int cols = Int32.Parse(itemXML.Element("BNNode").Element("Node").Element("CPT").Element("Columns").Value);
            string[] cptEnteries = itemXML.Element("BNNode").Element("Node").Element("CPT").Element("CPTTable").Value.Split(' ');
            // string[] noptEnteries = itemXML.Element("BNNode").Element("Node").Element("NOPT").Element("NOPTTable").Value.Split(' ');

            string[] castptEnteries = itemXML.Element("BNNode").Element("Node").Element("CASTPT").Element("CASTPTTable").Value.Split(' ');
            string[] stateEnteries = itemXML.Element("BNNode").Element("Node").Element("States").Value.Split(' ');
            string[] parentEnteries = itemXML.Element("BNNode").Element("Node").Element("Parents").Value.Split(',');
         

            item.BNNode.CPT.Columns = cols;
            int index = 0;

            //for adding states
            for (int i = 0; i < (stateEnteries.Length-1 ); i++)
                item.BNNode.AddState(stateEnteries[i]);

            // for adding CPT
            for (int i = 0; i < cols; i++)
            {
                //  
                for (int j = 0; j < rows; j++)
                    item.BNNode.CPT.SetValue(j, i, Double.Parse(cptEnteries[index++], CultureInfo.InvariantCulture));

            }

            //parents are added when network is initialized

            //for adding node type

            item.BNNode.Overriden = Convert.ToBoolean(itemXML.Element("BNNode").Element("Node").Element("Overriden").Value.ToString());
            switch (itemXML.Element("BNNode").Element("Node").Element("NodeType").Value.ToString())
            {

                case "General":
                    {
                        item.BNNode.NodeType = IBAyes.Bayesian.enmNodeType.General;
                        break;
                    }

                case "NoisyOR":
                    {
                      
                          item.BNNode.ChangeNodeType(IBAyes.Bayesian.enmNodeType.NoisyOR); 
                          int col = Int32.Parse(itemXML.Element("BNNode").Element("Node").Element("CASTPT").Element("Columns").Value);
                          item.BNNode.CASTPT.AdjustColumns(parentEnteries.Length-1);
         
                          //for adding CASTPT
                          item.BNNode.CASTPT.Columns = col;
                          int curIndex = 0;

                          for (int i = 2; i < 4; i++)
                          {
                              for (int j = 0; j <item.BNNode.CASTPT.Columns; j++)
                              {
                                  if (j % 2 != 1)
                                      item.BNNode.CASTPT.SetValue(i-2,j, Double.Parse(castptEnteries[curIndex++], CultureInfo.InvariantCulture));
                              }
                          }
                    
                        break;
                    }
                case "CAST":
                    {
                        item.BNNode.ChangeNodeType(IBAyes.Bayesian.enmNodeType.CAST);

                        item.BNNode.ChangeNodeType(IBAyes.Bayesian.enmNodeType.CAST);
                        int col = Int32.Parse(itemXML.Element("BNNode").Element("Node").Element("CASTPT").Element("Columns").Value);
                        item.BNNode.CASTPT.AdjustColumns(parentEnteries.Length - 1);
                        //for adding CASTPT
                        item.BNNode.CASTPT.Columns = col;
                        int curIndex = 0;

                        for (int i = 2; i < 3; i++)
                        {
                            for (int j = 0; j < item.BNNode.CASTPT.Columns; j++)
                            {
                               item.BNNode.CASTPT.SetValue(i - 2, j, Double.Parse(castptEnteries[curIndex++], CultureInfo.InvariantCulture));
                            }
                        }
                        break;
                    }
                case "NoisyMAX":
                    {
                        item.BNNode.NodeType = IBAyes.Bayesian.enmNodeType.NoisyMax;
                        break;
                    }

            }

            //evidence setting
            item.BNNode.EvidenceOn = Int32.Parse(itemXML.Element("BNNode").Element("Node").Element("hasEvidence").Value.ToString());
            if (item.BNNode.EvidenceOn > -1)
            {
                item.BNNode.SetEvidence(item.BNNode.EvidenceOn);
                //item.SetUnderlineProperty(item.BNNode.EvidenceOn);
            }


            return item;
        }

        private void CopyCurrentSelection()
        {
            IEnumerable<DesignerItem> selectedDesignerItems =
                this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            //IEnumerable<DesignerItem> selectedDesignerItems2 =
             //   this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            List<Connection> selectedConnections =
                this.SelectionService.CurrentSelection.OfType<Connection>().ToList();

           //

             

            foreach (Connection connection in this.Children.OfType<Connection>())
            {
                if ((!selectedConnections.Contains(connection)) || ( CopyUnSelectedConnection(connection) && (!selectedConnections.Contains(connection))))
                {
                    DesignerItem sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    DesignerItem sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null)
                        //BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }

                    if (sourceItem == null && sinkItem != null)
                    {
                        selectedConnections.Add(connection);
                    }
                  
                }
            }

            XElement designerItemsXML = SerializeDesignerItems(selectedDesignerItems);
            XElement connectionsXML = SerializeConnections(selectedConnections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }


        bool CopyUnSelectedConnection(Connection conn)
        {
            bool sourceSelected=false;
            bool sinkSelected=false;

            IEnumerable<DesignerItem> selectedDesignerItems =
                this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            foreach (DesignerItem designItem in selectedDesignerItems )
            { 
             if (conn.Sink.ParentDesignerItem.ID == designItem.ID)
                 sinkSelected = true;
             if (conn.Source.ParentDesignerItem.ID == designItem.ID)
                 sourceSelected = true;
             }
             if (sinkSelected & sourceSelected)
                    return true;
             else
                 return false;
        
        }
        private void DeleteCurrentSelection()
        {
            foreach (Connection connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                this.Children.Remove(connection);
                bnNetwork.DeleteConnection(connection.BNConnection);
            }

            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                Control cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                List<Connector> connectors = new List<Connector>();
                GetConnectors(cd, connectors);

                foreach (Connector connector in connectors)
                {
                    foreach (Connection con in connector.Connections)
                    {
                        this.Children.Remove(con);
                    }
                }
                this.Children.Remove(item);
                bnNetwork.DeleteNode(item.BNNode);

                Test objTest = (Test)GetMainWindow();
                StackPanel nodeItem;
                for (int i = 0; i < objTest.lstNodes.Items.Count; i++)
                {
                    nodeItem = (StackPanel)objTest.lstNodes.Items[i];
                    if (((TextBlock)nodeItem.Children[2]).Text == item.BNNode.Name)
                    {
                        ((Test)GetMainWindow()).lstNodes.Items.Remove(nodeItem);
                    }
                }
            }

            SelectionService.ClearSelection();
            UpdateZIndex();
        }

        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby Canvas.GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                Canvas.SetZIndex(ordered[i], i);
            }
        }

        private static Rect GetBoundingRectangle(IEnumerable<DesignerItem> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItem item in items)
            {
                x1 = Math.Min(Canvas.GetLeft(item), x1);
                y1 = Math.Min(Canvas.GetTop(item), y1);

                x2 = Math.Max(Canvas.GetLeft(item) + item.Width, x2);
                y2 = Math.Max(Canvas.GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        private void GetConnectors(DependencyObject parent, List<Connector> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                    GetConnectors(child, connectors);
            }
        }

        private Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();

            Control connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }

        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        public void SelectNode(string nodeName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.BNNode.Name == nodeName
                                         select item).FirstOrDefault();
            SelectionService.SelectItem(designerItem);
            designerItem.Focus();
        }

        #endregion



    }
}
