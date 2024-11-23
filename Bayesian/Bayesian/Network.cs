using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smile;
using Smile.Learning;
using System.Collections;
//using System.Collections.Specialized;

namespace IBAyes.Bayesian
{
    public class Network
    {
        ArrayList nodes;
        ArrayList connections;
        Smile.Network smileNetwork;
        enmBayesianAlgorithm algorithm;
        int sampleSize;
        enmNetworkType type;


        int nextNodeID, nextConnectionID;

        public Network()
        {
            smileNetwork = new Smile.Network();
            nodes = new ArrayList();
            connections = new ArrayList();
            nextNodeID = 1;
            nextConnectionID = 1;
            algorithm = enmBayesianAlgorithm.Lauritzan;
            type = enmNetworkType.BayesianNet;

        }

        public ArrayList Nodes { get { return nodes; } }

        public ArrayList Connections { get { return connections; } }

        public enmBayesianAlgorithm Algorithm 
        { get { return algorithm; }
            set { algorithm = value; } 
        }

        public enmNetworkType NetworkType{ get { return type; } set { type= value; } }


        public int SampleSize{ get { return sampleSize; } set { sampleSize= value; } }


        public Node CreateNode(enmNodeType nodeType)
        {
            Node newNode = new Node("Node_" + nextNodeID.ToString(), "Node " + nextNodeID.ToString(),nodeType);
            newNode.BNNetwork = this;
            nodes.Add(newNode);
            newNode.NodeHandle = smileNetwork.AddNode(Smile.Network.NodeType.Cpt,newNode.NodeID.ToString());
            nextNodeID++;

            newNode.AddState("T");
            newNode.AddState("F");

            newNode.InitializePT();
            return newNode;
        }

        //this is being called while loading existing BN file
        public Node CreateNode(string nodeId, string nodeName)
        {
            Node newNode = new Node(nodeId.ToString(), nodeName.ToString(), enmNodeType.General);
            newNode.BNNetwork = this;
            nodes.Add(newNode);
            newNode.NodeHandle = smileNetwork.AddNode(Smile.Network.NodeType.Cpt, newNode.NodeID.ToString());
            if (Int32.Parse(nodeId.Substring(5)) >= nextNodeID)
            { nextNodeID = Int32.Parse(nodeId.Substring(5)); nextNodeID++; }
            
            newNode.InitializeCASTPT();
            return newNode;
        }

        //this is called when copying a node

        public Node CreateNode()
        {
            Node newNode = new Node("Node_" + nextNodeID.ToString(), "Node " + nextNodeID.ToString());
            newNode.BNNetwork = this;
            nodes.Add(newNode);
            newNode.NodeHandle = smileNetwork.AddNode(Smile.Network.NodeType.Cpt, newNode.NodeID.ToString());
            nextNodeID++;

            newNode.InitializeCASTPT();
            return newNode;
        }

        public void DeleteNode(Node bnNode)
        {
            Node curNode;
            int i;

            //Remove connections with child nodes
            //for (i=0; i<bnNode.Chidren.Count; i++)
            while (bnNode.Chidren.Count>0)
            {
                curNode = (Node)bnNode.Chidren[0];
                DeleteConnection(GetConnectionObject(bnNode, curNode));
            }

            //Remove connection with parent nodes
            //for (i = 0; i < bnNode.Parents.Count; i++)
            while(bnNode.Parents.Count>0)
            {
                curNode = (Node)bnNode.Parents[0];
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
        public Node GetNodeObjectById(string nodeId)
        {
            foreach (Node n in nodes)
            {
                if (n.NodeID == nodeId)
                    return n;
            }
            return null;
        }

       public Connection CreateConnection(Node fromNode, Node toNode)
        {
           smileNetwork.AddArc(fromNode.NodeHandle, toNode.NodeHandle);

            Connection newConnection = new Connection(nextConnectionID);
            newConnection.SourceNode = fromNode;
            newConnection.SinkNode = toNode;
            newConnection.BNNetwork = this;
            connections.Add(newConnection);
            fromNode.Chidren.Add(toNode);
            toNode.Parents.Add(fromNode);

            toNode.InsertParent(fromNode);

            if (toNode.NodeType == enmNodeType.NoisyMax)
                toNode.NOPT.InsertParent(fromNode,-1);

            if (toNode.NodeType == enmNodeType.CAST ^ toNode.NodeType == enmNodeType.NoisyOR)
                toNode.CASTPT.InsertParent(fromNode);

            nextConnectionID++;
            if (toNode.NodeType == enmNodeType.CAST || toNode.NodeType == enmNodeType.NoisyOR)
            {
                toNode.GenerateCPTforCASTNode();
            }
            return newConnection;
        }

       public Connection CreateConnection2(Node fromNode, Node toNode)
       {
           Connection newConnection = new Connection(nextConnectionID);
           newConnection.SourceNode = fromNode;
           newConnection.SinkNode = toNode;
           newConnection.BNNetwork = this;
           connections.Add(newConnection);
           fromNode.Chidren.Add(toNode);
           toNode.Parents.Add(fromNode);

           smileNetwork.AddArc(fromNode.NodeHandle, toNode.NodeHandle);

           /*toNode.InsertParent(fromNode);

           if (toNode.NodeType == enmNodeType.NoisyMax)
               toNode.NOPT.InsertParent(fromNode, -1);

           if (toNode.NodeType == enmNodeType.CAST ^ toNode.NodeType == enmNodeType.NoisyOR)
               toNode.CASTPT.InsertParent(fromNode);
           */
           nextConnectionID++;
           return newConnection;
       }

        public void DeleteConnection(Connection connection)
       {
           if (connection.SinkNode.NodeType == enmNodeType.NoisyMax)
           {
               connection.SinkNode.NOPT.RemoveColumn(connection.SourceNode, -1);
           }

           if (connection.SinkNode.NodeType == enmNodeType.CAST ^ connection.SinkNode.NodeType == enmNodeType.NoisyOR)
           {
               connection.SinkNode.CASTPT.RemoveColumn(connection.SourceNode, -1);
           }

           connection.SinkNode.CPT.RemoveColumn(connection.SourceNode);

           smileNetwork.DeleteArc(connection.SourceNode.NodeHandle, connection.SinkNode.NodeHandle);
           connections.Remove(connection);
           connection.SourceNode.Chidren.Remove(connection.SinkNode);
           connection.SinkNode.Parents.Remove(connection.SourceNode);
       }


        public Smile.Network SmileNetwork { get { return smileNetwork; } }

        public void UpdateBelief()
        {
            int i;    
            double[] probabs;
            probabs = new double[100];

            PT table;

            if (algorithm == enmBayesianAlgorithm.BackwardSampling)
                smileNetwork.BayesianAlgorithm = Smile.Network.BayesianAlgorithmType.BackSampling;
            else if (algorithm == enmBayesianAlgorithm.EPISSampling)
                smileNetwork.BayesianAlgorithm = Smile.Network.BayesianAlgorithmType.EpisSampling;
            else if (algorithm == enmBayesianAlgorithm.HenrionSampling)
                smileNetwork.BayesianAlgorithm = Smile.Network.BayesianAlgorithmType.Henrion;
            else if (algorithm == enmBayesianAlgorithm.Lauritzan)
                smileNetwork.BayesianAlgorithm = Smile.Network.BayesianAlgorithmType.Lauritzen;
            else if (algorithm == enmBayesianAlgorithm.LikelihoodSampling)
                smileNetwork.BayesianAlgorithm = Smile.Network.BayesianAlgorithmType.LSampling;
            else if (algorithm == enmBayesianAlgorithm.SelfImportance)
                smileNetwork.BayesianAlgorithm = Smile.Network.BayesianAlgorithmType.SelfImportance;

            smileNetwork.SampleCount = sampleSize;

            //Update CPTs for each node
            foreach (Node node in nodes)
            {
                table = node.CPT;
                if (node.NodeType == enmNodeType.NoisyMax)
                    table = node.NOPT;

                probabs = new double[table.Columns * table.Rows];

                int curIndex = 0;

                curIndex = 0;
                for (i = 0; i < table.Columns; i++)
                {
                    for (int j = 0; j < table.Rows; j++)
                    {
                        probabs[curIndex++] = table.GetValue(j, i);
                    }
                }

                smileNetwork.SetNodeDefinition(node.NodeHandle, probabs);
            }

            //Update all beliefs 
            smileNetwork.UpdateBeliefs();



            //Print updated belifes on console
            foreach (Node node in nodes)
            {
                //Console.WriteLine(node.Name + " : ");
                if (smileNetwork.IsEvidence(node.NodeHandle))  ///If smile has (either real or not real)evidence on this node then automaticlaly set posterior probab to 1
                {
                    int smileEvidenceOn = smileNetwork.GetEvidence(node.NodeHandle);
                    //TODO: temporary fix for a smile problem     
                    for (i = 0; i < node.NoOfStates; i++)
                    {
                        if (i == smileEvidenceOn)
                        {
                            node.SetPosteriorProbab(i, 1);
                        }
                        else
                        {
                            node.SetPosteriorProbab(i, 0);
                       }
                    }
                }
                else
                {
                    double[] arr = new double[node.NoOfStates];
                    arr = smileNetwork.GetNodeValue(node.NodeHandle);
                    for (i = 0; i < node.NoOfStates; i++)
                    {
                        node.SetPosteriorProbab(i,arr[i]);
                    }
                }
            }
        }

        public void ClearAllEvidences()
        {
            foreach (Node node in Nodes)
            {
                if (node.EvidenceOn >= 0)
                {
                    node.ClearEvidence();
                }
            }
        }

        public Hashtable PerformSensitivityAnalysis(Node focusNode)
        {
            Hashtable saResult = new Hashtable();
            double baseline;

            if (!focusNode.IsRootNode)
            {

                ClearAllEvidences();
                UpdateBelief();
                baseline = focusNode.GetPosteriorProbab(0);

                foreach (Node node in nodes)
                {
                    if ((node.IsRootNode) & (node.NodeID != focusNode.NodeID))
                    {
                        SAResult result = new SAResult();
                        result.NodeID = node.NodeID;
                        result.NodeName = node.Name;
                        result.Baseline = baseline;

                        node.SetEvidence(0);
                        UpdateBelief();
                        result.ProbT = Math.Round(focusNode.GetPosteriorProbab(0), 4);

                        node.SetEvidence(1);
                        UpdateBelief();
                        result.ProbF = Math.Round(focusNode.GetPosteriorProbab(0), 4);

                        result.EntT = Math.Round((result.ProbT * Math.Log(result.ProbT, Math.Exp(1))) - ((1 - result.ProbT) * Math.Log((1 - result.ProbT), Math.Exp(1))), 4);
                        result.EntF = Math.Round((result.ProbF * Math.Log(result.ProbF, Math.Exp(1))) - ((1 - result.ProbF) * Math.Log((1 - result.ProbF), Math.Exp(1))),4);

                        saResult.Add(node.NodeID, result);

                        node.ClearEvidence();
                    }
                }
            }
            return saResult;
        }

        public Hashtable PerformSensitivityToInfluence(Node focusNode)
        {
            Hashtable saResult = new Hashtable();
            double baseline, gValue, hValue, minValue, maxValue;
            Node source, sink;

            if (!focusNode.IsRootNode)
            {

                ClearAllEvidences();
                UpdateBelief();
                baseline = focusNode.GetPosteriorProbab(0);

                foreach (Connection connection in connections)
                {
                    source = connection.SourceNode;
                    sink = connection.SinkNode;

                    if (sink.NodeType == enmNodeType.CAST)
                    {
                        gValue = sink.GetGValue(source);
                        hValue = sink.GetHValue(source);

                        SAResult result = new SAResult();
                        result.NodeID = source.NodeID;
                        result.NodeName = source.Name;
                        result.ToNodeName = sink.Name;
                        result.Baseline = baseline;

                        if (gValue >= 0)
                        { minValue = 0; maxValue = 0.99; }
                        else
                        { minValue = -0.99; maxValue = 0; }

                        sink.SetGValue(source, minValue);
                        sink.GenerateCPTforCASTNode();
                        UpdateBelief();
                        result.ProbF = Math.Round(focusNode.GetPosteriorProbab(0), 4);

                        sink.SetGValue(source, maxValue);
                        sink.GenerateCPTforCASTNode();
                        UpdateBelief();
                        result.ProbT = Math.Round(focusNode.GetPosteriorProbab(0), 4);

                        if (hValue >= 0)
                        { minValue = 0; maxValue = 0.99; }
                        else
                        { minValue = -0.99; maxValue = 0; }

                        sink.SetHValue(source, minValue);
                        sink.GenerateCPTforCASTNode();
                        UpdateBelief();
                        result.ProbH_F = Math.Round(focusNode.GetPosteriorProbab(0), 4);

                        sink.SetGValue(source, maxValue);
                        sink.GenerateCPTforCASTNode();
                        UpdateBelief();
                        result.ProbH_T = Math.Round(focusNode.GetPosteriorProbab(0), 4);

                        saResult.Add(connection.ID.ToString(), result);

                        //Restore original values
                        sink.SetGValue(source, gValue);
                        sink.SetHValue(source, hValue);
                        sink.GenerateCPTforCASTNode();
                    }
                }
            }
            return saResult;
        }
    }

    public class SAResult
    {
        public string NodeID;
        public string NodeName;
        public string ToNodeName;
        public double Baseline;
        public double ProbT;
        public double ProbF;
        public double ProbH_T;
        public double ProbH_F;
        public double EntT;
        public double EntF;
    }

    public enum enmBayesianAlgorithm
    {
        Lauritzan,
        LikelihoodSampling,
        BackwardSampling,
        EPISSampling,
        HenrionSampling,
        SelfImportance
    }

    public enum enmNetworkType
    {
        BayesianNet,
        InfluenceNet
    }

}