using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIBAL.Bayesian
{
    public class CASTNode:Node
    {
         CASTPT _CASTPT;

         public CASTNode(Node node)
             : base(node)
         {
             _CASTPT = new CASTPT(this);
         }

        public CASTPT CASTPT
        {
            get { return _CASTPT; }
        }

        //public override void AddState(string stateName)
        //{
        //    _CASTPT.AddRow();

        //    foreach (Node node in _childNodes)
        //    {
        //        if (node.NodeType == enmNodeType.CAST)
        //        {
        //            _CASTPT.InsertParent(this);
        //        }
        //    }

        //    base.AddState(stateName);
        //}

        public double GetValue(int row, int col)

        {
            return _CASTPT.GetValue(row, col);
        }

        public void SetValue(int row, int col, double value)
        {
            _CASTPT.SetValue(row, col, value);
        }

        public void RemoveParent(Node parentNode, int stateindex)
        {
            _CASTPT.RemoveColumn(parentNode, stateindex);
            base.RemoveParent(parentNode, stateindex);
        }

        public void InsertParent(Node parentNode)
        {
            _CASTPT.InsertParent(parentNode);
            base.InsertParent(parentNode);
        }
    }
}
