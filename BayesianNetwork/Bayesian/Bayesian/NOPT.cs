using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBAyes.Bayesian
{
    public class NOPT:PT
    {
        internal NOPT(Node curNode): base(curNode)
        {
            int i;
            List<int> colIndex = new List<int>(); 

            cols = 0;

            for (i = 0; i < node.Parents.Count; i++)
            {
                colIndex.Add(cols + ((Node)node.Parents[i]).NoOfStates-1 );
                cols += ((Node)node.Parents[i]).NoOfStates;
            }

            //Adding one more column for Leak
            cols++;
            colIndex.Add(cols-1);

            for (i = 0 ;i < node.NoOfStates ; i++)
            {               
                AddRow();
            }

            for (i = 0; i < colIndex.Count; i++)
            {
                SetValue(node.NoOfStates - 1, colIndex[i], 1);
            }
        }

        public void AdjustColumns()
        {
            int i, newColumnCount = 0;
            for (i = 0; i < node.Parents.Count; i++)
            {
                newColumnCount += ((Node)node.Parents[i]).NoOfStates;
            }
            newColumnCount++;

            if (cols < newColumnCount)
            {
                for (i = 0; i < cptTable.Count; i++)
                {
                    for (int j = 0; j < newColumnCount - cols; j++)
                        cptTable[i].Add(0.0);
                }
            }
            cols = newColumnCount;

        }

        public void InsertParent(Node parentNode, int stateIndex)
        {
            int i,j, colCount=0, newParentIndex;

            newParentIndex = node.Parents.IndexOf(parentNode);

            for (i = 0; i < newParentIndex; i++)
            {
                colCount += ((Node)node.Parents[i]).NoOfStates;
            }
            if (colCount == 0)
                colCount = 1;

            for (i = 0; i < cptTable.Count; i++)
            {
                if (stateIndex == -1)
                {
                    for (j = 0; j < parentNode.NoOfStates; j++)
                    {
                        cptTable[i].Insert(colCount - 1 + j, 0.0);
                    }
                }
                else if (stateIndex > 0)
                {
                    cptTable[i].Insert(colCount - 1 + stateIndex, 0.0);
                }
            }
            cols = cols + (stateIndex == -1? parentNode.NoOfStates : 1);
        }        

        internal List<int> GetColumnIndex(int parentIndex, int stateIndex)
        {
            List<int> lstIndexes = new List<int>();

            int i, colsBefore = 0;
            for (i = 0; i < parentIndex; i++)
            {
                colsBefore += ((Node)node.Parents[i]).NoOfStates;
            }

            for (i = 0; i < ((Node)node.Parents[parentIndex]).NoOfStates; i++)
            {
                if ( (stateIndex == -1) ^ (stateIndex ==i))
                {
                    lstIndexes.Add(colsBefore+i);
                }
            }

            return lstIndexes;

        }

        internal void RemoveColumn(Node parentNode, int stateIndex)
        {
            List<int> colIndexes = GetColumnIndex(node.Parents.IndexOf(parentNode), stateIndex);
            for (int r = 0; r < Rows; r++)
            {
                for (int c = colIndexes.Count - 1; c >= 0; c--)
                {
                    cptTable[r].RemoveAt(colIndexes[c]);
                }
            }
            cols = cols - colIndexes.Count;
        }
    }
}
