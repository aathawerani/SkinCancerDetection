using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagramDesigner.Bayesian
{
    public class Node
    {
     private String nodeID;
     private String nodeName;
     private int nodeHandle;
     StringCollection states;
     int hasEvidenceOn;
     
     private Network bnNetwork;
     private ArrayList parentNodes;
     private ArrayList childNodes;
     private CPT nodeCPT;

     public Node(String ID, String Name)
     {
         nodeID = ID;
         nodeName = Name;
         states = new StringCollection();
         parentNodes = new ArrayList();
         childNodes = new ArrayList();
         nodeCPT = new CPT(this);
         hasEvidenceOn = -1;
     }

        #region Properties

        public CPT CPT
        {
            get { return nodeCPT; }
        }

        public String NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public String Name
        {
            get{return nodeName;}
            set{nodeName=value;}
        }

        public StringCollection States
        {
            get{return states;}
        }

        public int NodeHandle
        {
            get { return nodeHandle; }
            set { nodeHandle = value; }
        }

        public Bayesian.Network BNNetwork
        {
            get { return bnNetwork; }
            set { bnNetwork = value; }
        }

        public int NoOfStates
        {
            get { return states.Count; }
        }

        public ArrayList Chidren
        {
            get { return childNodes; }
            set { childNodes = value; }
        }

        public ArrayList Parents
        {
            get { return parentNodes; }
            set { parentNodes = value; }
        }
        public int EvidenceOn
        {
            get { return hasEvidenceOn; }
        }

            #endregion

        # region Methods

        public void AddState(string stateName)
        {
            states.Add(stateName);
            
            if (NoOfStates <= 2)
            {
                bnNetwork.SmileNetwork.SetOutcomeId(nodeHandle, NoOfStates - 1, stateName);
            }

            else
            {
                bnNetwork.SmileNetwork.AddOutcome(nodeID, stateName);
            }

            CPT.AddRow();

            foreach (Node node in this.childNodes)
            {
                node.CPT.AdjustColumns();
            }
        }

        public void RemoveState(string stateName)
        {
            int index = States.IndexOf(stateName);

            if (NoOfStates > 2)
            {
                foreach (Node node in childNodes)
                {
                    node.CPT.RemoveColumn(this, index);
                }    

                bnNetwork.SmileNetwork.DeleteOutcome(nodeID, stateName);
                states.RemoveAt(index);
                CPT.RemoveRow(index);
            }
        }

        public void SetEvidence(int stateIndex)
        {
            hasEvidenceOn = stateIndex;
            bnNetwork.SmileNetwork.SetEvidence(nodeHandle,stateIndex);
        }

        public void ClearEvidence()
        {
            bnNetwork.SmileNetwork.ClearEvidence(nodeHandle);
            hasEvidenceOn = -1;
        }
        
        #endregion

    }
}
