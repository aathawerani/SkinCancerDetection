using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smile;
using Smile.Learning;
using DiagramDesigner.Bayesian;
using System.Collections;

namespace DiagramDesigner.Bayesian
{
    public class Network
    {
        ArrayList nodes;
        ArrayList connections;
        Smile.Network smileNetwork;
        int nextNodeID, nextConnectionID;

        public Network()
        {
            smileNetwork = new Smile.Network();
            nodes = new ArrayList();
            connections = new ArrayList();
            nextNodeID = 1;
            nextConnectionID = 1;
            
        }

        //public int NextNodeID { get { return nextNodeID; } }
        //public int NextConnectionID { get { return nextConnectionID; } }
        public ArrayList Nodes { get { return nodes; } }


        public Node CreateNode()
        {
            Node newNode = new Node("Node_" + nextNodeID.ToString(), "Node " + nextNodeID.ToString());
            newNode.BNNetwork = this;
            nodes.Add(newNode);
            newNode.NodeHandle = smileNetwork.AddNode(Smile.Network.NodeType.Cpt,newNode.NodeID.ToString());
            nextNodeID++;

            newNode.AddState("T");
            newNode.AddState("F");

            return newNode;
        }

        public void DeleteNode(Node bnNode)
        {
            Node curNode;
            int i;

            //Remove connections with child nodes
            for (i=0; i<bnNode.Chidren.Count; i++)
            {
                curNode = (Node)bnNode.Chidren[i];
                DeleteConnection(GetConnectionObject(bnNode, curNode));
            }

            //Remove connection with parent nodes
            for (i = 0; i < bnNode.Parents.Count; i++)
            {
                curNode = (Node)bnNode.Parents[i];
                curNode.Chidren.Remove(bnNode);
                DeleteConnection(GetConnectionObject(curNode, bnNode));
            }

            //Delete node from smile network
            smileNetwork.DeleteNode(bnNode.NodeHandle);

            //Delete node itself
            nodes.Remove(bnNode);
        }

        private Connection GetConnectionObject(Node Source, Node Sink)
        {
            foreach (Connection conn in connections)
            {
                if ((conn.SourceNode.NodeID == Source.NodeID) && (conn.SinkNode.NodeID == Sink.NodeID))
                    return conn;
            }
            return null;
        }

       public Connection CreateConnection(Node fromNode, Node toNode)
        {
            Connection newConnection = new Connection(nextConnectionID);
            newConnection.SourceNode = fromNode;
            newConnection.SinkNode = toNode;
            newConnection.BNNetwork = this;
            connections.Add(newConnection);
            fromNode.Chidren.Add(toNode);
            toNode.Parents.Add(fromNode);

            smileNetwork.AddArc(fromNode.NodeHandle, toNode.NodeHandle);

            toNode.CPT.AdjustColumns();

            nextConnectionID++;
            return newConnection;
        }

       public void DeleteConnection(Connection connection)
       {
           smileNetwork.DeleteArc(connection.SourceNode.NodeHandle, connection.SinkNode.NodeHandle);
           connections.Remove(connection);
           connection.SourceNode.Chidren.Remove(connection.SinkNode);
           connection.SinkNode.Parents.Remove(connection.SourceNode);
           
           //Adjust CPT of destination node
           connection.SinkNode.CPT.AdjustColumns();
       }


        public Smile.Network SmileNetwork { get { return smileNetwork; } }

        public void UpdateBelief()
        {
            int i;    
            double[] probabs;
            probabs = new double[100];
            

            //Update CPTs for each node
            foreach (Node node in nodes)
            {
                probabs = new double[node.CPT.Columns * node.CPT.Rows];

                int curIndex = 0;

                curIndex = 0;
                for (i = 0; i < node.CPT.Columns; i++)
                {
                    for (int j = 0; j < node.CPT.Rows; j++)
                    {
                        probabs[curIndex++] = node.CPT.GetValue(j, i); 
                    }
                }

                smileNetwork.SetNodeDefinition(node.NodeHandle, probabs);
            }

            //Update all beliefs 
            smileNetwork.UpdateBeliefs();

            //Print updated belifes on console
            foreach (Node node in nodes)
            {
                Console.WriteLine(node.Name + " : ");
                if (node.EvidenceOn >= 0)
                {
                    //TODO: temporary fix for a smile problem     
                    for (i = 0; i < node.NoOfStates; i++)
                    {
                        if (i == node.EvidenceOn )
                            Console.WriteLine("    " + node.States[i] + " : 1");
                        else
                            Console.WriteLine("    " + node.States[i] + " : 0");
                    }
                }
                else
                {
                    double[] arr = new double[node.NoOfStates];
                    arr = smileNetwork.GetNodeValue(node.NodeHandle);
                    for (i = 0; i < node.NoOfStates; i++)
                    {
                        Console.WriteLine("    " + node.States[i] + " : " + arr[i].ToString());
                    }
                }
            }
        }


    }

}