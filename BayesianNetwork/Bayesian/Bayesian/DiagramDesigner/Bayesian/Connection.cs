using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiagramDesigner.Bayesian;

namespace DiagramDesigner.Bayesian
{
    public class Connection
    {
        int connectionID;
        Node sourceNode, sinkNode;
        Network bnNetwork;

        public Connection(int ID)
        {
            connectionID = ID;
        }
        public int ID
        {
            get { return connectionID; }
            set { connectionID = value; }
        }

        public Node SourceNode
        {
            get{return sourceNode;} 
            set{sourceNode=value;}
        }

        public Node SinkNode
        {
            get { return sinkNode; }
            set { sinkNode = value; }
        }

        public Bayesian.Network BNNetwork
        {
            get { return bnNetwork; }
            set { bnNetwork = value; }
        }

    }
} 
