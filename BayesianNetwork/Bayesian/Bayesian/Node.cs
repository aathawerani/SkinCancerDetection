using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBAyes.Bayesian
{
    public class Node
    {
         protected String _nodeID;
         protected String _nodeName;
         protected int _nodeHandle;
         protected enmNodeType _nodeType;
         protected int _hasEvidenceOn;
         protected StringCollection _states;
         protected List<double> _posteriorProbab;
         protected Network _bnNetwork;
         protected ArrayList _parentNodes;
         protected ArrayList _childNodes;
         protected CPT _nodeCPT;
         protected NOPT _NOPT;
         protected CASTPT _CASTPT;
         bool _overriden;

        //this construcotr is called when node is dragged from toolbar and dropped on canvas.
         public Node(String ID, String Name,enmNodeType nodeType)
         {
             _nodeID = ID;
             _nodeName = Name;
             _states = new StringCollection();
             _posteriorProbab = new List<double>();
             _parentNodes = new ArrayList();
             _childNodes = new ArrayList();
             _nodeCPT = new CPT(this);
             _hasEvidenceOn = -1;
             _nodeType = nodeType;
             _overriden = false;

             //if (nodeType == enmNodeType.NoisyMax)
             //    _NOPT = new NOPT(this);

             //else if(nodeType == enmNodeType.CAST ^ nodeType == enmNodeType.NoisyOR)
             //    _CASTPT = new CASTPT(this);
         }


         //this constructor is called when an existing BN file is opened 
         public Node(String ID, String Name)
         {
             _nodeID = ID;
             _nodeName = Name;
             _states = new StringCollection();
             _posteriorProbab = new List<double>();
             _parentNodes = new ArrayList();
             _childNodes = new ArrayList();
             _nodeCPT = new CPT(this);
             _hasEvidenceOn = -1;
             _nodeType = enmNodeType.General;
             if (_nodeType == enmNodeType.NoisyMax)
                 _NOPT = new NOPT(this);


         }

        #region Properties

        public CPT CPT
        {
            get { return _nodeCPT; }
        }

        public NOPT NOPT
        {
            get { return _NOPT; }
        }

        public CASTPT CASTPT
        {
            get { return _CASTPT; }
        }

        public String NodeID
        {
            get { return _nodeID; }
            set { _nodeID = value; }
        }

        public String Name
        {
            get{return _nodeName;}
            set{_nodeName=value;}
        }
        
        public enmNodeType NodeType
        {
            get { return _nodeType; }
            set {
                _nodeType = value;
            }
        }

        public StringCollection States
        {
            get{return _states;}
        }

        public int NodeHandle
        {
            get { return _nodeHandle; }
            set { _nodeHandle = value; }
        }

        public Bayesian.Network BNNetwork
        {
            get { return _bnNetwork; }
            set { _bnNetwork = value; }
        }

        public int NoOfStates
        {
            get { return _states.Count; }
        }

        public ArrayList Chidren
        {
            get { return _childNodes; }
            set { _childNodes = value; }
        }

        public ArrayList Parents
        {
            get { return _parentNodes; }
            set { _parentNodes = value; }
        }
        public int EvidenceOn
        {
            get { return _hasEvidenceOn; }
            set { _hasEvidenceOn = value; }
        }

        public bool Overriden
        {
            get { return _overriden; }
            set { _overriden = value; }
        }

        public bool IsRootNode
        {
            get { if (Parents.Count > 0)  return false; else return true ; }
        }

        public bool IsLeafNode
        {
            get { if (Chidren.Count > 0)  return false; else return true; }
        }


        #endregion

        # region Methods


        internal void InitializePT()
        {
            _NOPT = new NOPT(this);
            _CASTPT = new CASTPT(this);        

            if (_nodeType == enmNodeType.General)
            {
                CPT.SetValue(0, 0, 0.5);
                CPT.SetValue(1, 0, 0.5);
            }

            if (_nodeType == enmNodeType.NoisyOR)
            {
                CASTPT.SetValue(0, 0, .1);
                CASTPT.SetValue(1, 0, .9);
                GenerateCPTforCASTNode();
            }
            if (_nodeType == enmNodeType.CAST)
            {
                CASTPT.SetValue(0, 0, 0.1);
                //CASTPT.SetValue(1, 0, .5);
                GenerateCPTforCASTNode();
            }
        }
       
       
        internal void InitializeCASTPT()
        {
           // _NOPT = new NOPT(this);
            _CASTPT = new CASTPT(this);
        }



        public void AddState(string stateName)
        {
            
            _states.Add(stateName);
            _posteriorProbab.Add(0);

            
            if (NoOfStates <= 2)
            {
                _bnNetwork.SmileNetwork.SetOutcomeId(_nodeHandle, NoOfStates - 1, stateName);
            }

            else
            {
                _bnNetwork.SmileNetwork.AddOutcome(_nodeID, stateName);
            }

            CPT.AddRow();

            if (NodeType == enmNodeType.NoisyMax)
            {
                NOPT.AddRow();
            }
            //else if (NodeType == enmNodeType.CAST)
            //{
            //    CASTPT.AddRow();
            //}

            foreach (Node node in this._childNodes)
            {
                node.CPT.AdjustColumns();

                if (node.NodeType == enmNodeType.NoisyMax)
                {
                    node.NOPT.InsertParent(this, _states.Count - 1);
                }
                //if (node.NodeType == enmNodeType.CAST)
                //{
                //    node.CASTPT.InsertParent(this,);
                //}
            }
        }

        public void RemoveState(string stateName)
        {
            int index = States.IndexOf(stateName);

            if (NoOfStates > 2)
            {
                foreach (Node node in _childNodes)
                {
                    node.CPT.RemoveColumn(this, index);
                    if (node.NodeType == enmNodeType.NoisyMax)
                    {
                        node.NOPT.RemoveColumn(this, index);
                    }
                    if (node.NodeType == enmNodeType.CAST)
                    {
                        node.CASTPT.RemoveColumn(this, index);
                    }
                }    

                _bnNetwork.SmileNetwork.DeleteOutcome(_nodeID, stateName);
                _states.RemoveAt(index);
                CPT.RemoveRow(index);
            }
        }

        public void SetEvidence(int stateIndex)
        {
            try
            {
                //if (_bnNetwork.SmileNetwork.IsEvidence(_nodeID))
                _bnNetwork.SmileNetwork.SetEvidence(_nodeHandle, stateIndex);
                _hasEvidenceOn = stateIndex;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Can't set evidence to outcome"))
                {
                    throw new Exception("Can not set evidence. There is a already propagated evidence on this node.");
                }
  
            }

        }

        internal void SetPosteriorProbab(int state, double probab)
        {
            _posteriorProbab[state] = probab;
        }

        public double GetPosteriorProbab(int state)
        {
            return _posteriorProbab[state];
        }

        public void ClearEvidence()
        {
            if (_hasEvidenceOn > -1)
            {
                _bnNetwork.SmileNetwork.ClearEvidence(_nodeHandle);
                _hasEvidenceOn = -1;
            }
        }

        public Node ChangeNodeType(enmNodeType newNodeType)
        {
            if (newNodeType == enmNodeType.General && Parents.Count > 9)
            {
                throw new Exception("Node Type can not be changed. IBAYES does not allow General node to have more than 9 parents.");
            }

            if (_nodeType != enmNodeType.NoisyMax & newNodeType == enmNodeType.NoisyMax)
            {
                _nodeType = newNodeType;
                _NOPT = new NOPT(this);
                _bnNetwork.SmileNetwork.SetNodeType(_nodeHandle, Smile.Network.NodeType.NoisyMax);
            }

            else if (_nodeType == enmNodeType.NoisyMax & newNodeType != enmNodeType.NoisyMax)
            {
                _NOPT = null;
            }

            if (_nodeType != enmNodeType.CAST & _nodeType !=enmNodeType.NoisyOR & (newNodeType == enmNodeType.CAST ^ newNodeType == enmNodeType.NoisyOR) )
            {
                _nodeType = newNodeType;
                _CASTPT = new CASTPT(this);
                _bnNetwork.SmileNetwork.SetNodeType(_nodeHandle, Smile.Network.NodeType.Cpt);

                for (int i = 0; i < _CASTPT.Columns-1; i++)
                {
                    _CASTPT.SetValue(0, i, 0);

                    if (_nodeType == enmNodeType.NoisyOR)
                    {
                        _CASTPT.SetValue(1, i, 1);
                    }
                }
                _CASTPT.SetValue(0, _CASTPT.Columns-1, 0.1);

                if (_nodeType == enmNodeType.NoisyOR)
                {
                    _CASTPT.SetValue(1, _CASTPT.Columns-1, 0.9);
                }

            }
            else if ( ( _nodeType == enmNodeType.CAST ^ _nodeType == enmNodeType.NoisyOR) & newNodeType != enmNodeType.CAST & newNodeType != enmNodeType.NoisyOR)
            {
                //Need to clear CASTPT but it must NOT be null
                _CASTPT = new CASTPT(this);

            }

            if ( (_nodeType == enmNodeType.CAST) & (newNodeType == enmNodeType.NoisyOR) )
            {
                CASTPT.AddRow();
                for (int j = 0; j < CASTPT.Columns-1; j++)
                {
                    if (j % 2 == 1)
                    {
                        CASTPT.SetValue(0, j, 0);
                        CASTPT.SetValue(1, j, 0);
                    }
                    else
                    {
                        CASTPT.SetValue(1, j, 1 - CASTPT.GetValue(0, j));
                    }
                }
                CASTPT.SetValue(0, CASTPT.Columns - 1, 0.1);
                CASTPT.SetValue(1, CASTPT.Columns - 1, 0.9);
            }
            if ((_nodeType == enmNodeType.NoisyOR) & (newNodeType == enmNodeType.CAST))
            {
                CASTPT.RemoveRow(1);
                for (int j = 0; j < CASTPT.Columns - 1; j++)
                {
                    CASTPT.SetValue(0, j, 0);
                }
                CASTPT.SetValue(0, CASTPT.Columns - 1, 0.1);
            }

            //if ( (_nodeType == enmNodeType.CAST & newNodeType == enmNodeType.NoisyOR ) ^ (_nodeType == enmNodeType.NoisyOR & newNodeType == enmNodeType.CAST ) )
            //{
            //    
            //}
            _nodeType = newNodeType;
            return this;
        }

        public void SetValue(int row, int col, double value)
        {
            CPT.SetValue(row, col, value);
        }

        public double GetValue(int row, int col)
        {
            return CPT.GetValue(row, col);
        }

        public void RemoveParent(Node parentNode, int stateIndex)
        {
            CPT.RemoveColumn(parentNode);
        }

        public void InsertParent(Node parentNode)
        {
            CPT.AdjustColumns();
        }

        public string[] GetAllParents()
        {
            string[] str;

            str = new string[2 * Parents.Count];
            int j = 0;

            foreach (Node n in Parents)
            {
                str[j++] = n.NodeID;
                str[j++] = ",";

            }

            return str;
        }

        public string[] GetAllStates()
        {
            string[] str;
            str = new string[NoOfStates + NoOfStates];
            int j = 0;
            for (int i = 0; i < NoOfStates; i++)
            {
                str[j++] = States[i];
                str[j++] = " ";
            }
            return str;
        }

        public void GenerateCPTforCASTNode()
        {
            int i, j, k, colspan, totalCols = 1;
            Node curParent;


            for (i = 0; i < Parents.Count; i++)
            {
                totalCols = totalCols * ((Node)Parents[i]).NoOfStates;
            }

            int[,] arr = new int[totalCols, Parents.Count];
            colspan = totalCols;
            for (i = 0; i < Parents.Count; i++)
            {
                curParent = (Node)Parents[i];
                colspan = colspan / curParent.NoOfStates;

                //for each column, display column  heading as state/outcome of node's parents.
                for (k = 0, j = 0; j < totalCols; j++)
                {
                    k = j / colspan;

                    if (k >= curParent.NoOfStates)
                        k = k % curParent.NoOfStates;

                    arr[j, i] = k;
                }
            }

            double PI, NI, AI, value;



            for (j = 0; j < totalCols; j++)
            {
                PI = 1;
                NI = 1;
                for (i = 0; i < Parents.Count; i++)
                {
                    value = _CASTPT.GetValue(0, +((i) * 2) + arr[j, i]);
                    if (value >= 0)
                        PI = PI * (1 - value);
                    else
                        NI = NI * (1 - Math.Abs(value));
                }

                PI = 1 - PI;
                NI = 1 - NI;

                AI = Math.Abs(PI - NI) / (1 - Math.Min(PI, NI));

                double baseline, CP = 0;
                baseline = CASTPT.GetValue(0, CASTPT.Columns - 1);

                if (PI > NI)
                {
                    CP = baseline + ((1 - baseline) * AI);
                }
                else if (PI < NI)
                {
                    CP = baseline - (baseline * AI);
                }
                else if (PI == NI)
                {
                    CP = baseline;
                }

                CPT.SetValue(0, j, Math.Round(CP, 3));
                CPT.SetValue(1, j, 1 - Math.Round(CP, 3));
            }
        }

        public double GetGValue(Node parent)
        {
            int parentIndex = Parents.IndexOf(parent);
            return CASTPT.GetValue(0, parentIndex * 2);
        }

        public double GetHValue(Node parent)
        {
            int parentIndex = Parents.IndexOf(parent);
            return CASTPT.GetValue(0, parentIndex * 2 + 1);
        }

        public void SetGValue(Node parent, double g)
        {
            int parentIndex = Parents.IndexOf(parent);
            CASTPT.SetValue(0, parentIndex * 2,g);
        }

        public void SetHValue(Node parent, double h)
        {
            int parentIndex = Parents.IndexOf(parent);
            CASTPT.SetValue(0, parentIndex * 2 +1, h);
        }
 
        #endregion

    }

    public enum enmNodeType
    {
        General,
        CAST,
        NoisyOR,
        NoisyMax
    }
}
