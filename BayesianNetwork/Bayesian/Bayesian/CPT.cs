using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBAyes.Bayesian
{
    public class CPT: PT
    {
        internal CPT(Node curNode): base(curNode)
        {

        }


        internal List<int> GetColumnIndex(int parentIndex, int stateIndex)
        {
            int colspan, totalcols;
            Node curParent;
            List<int> lstIndexes = new List<int>();

            totalcols = Columns;
            colspan = totalcols;

            for (int i = 0; i < node.Parents.Count; i++)
            {
                curParent = (Node)node.Parents[i];
                colspan = colspan / curParent.NoOfStates;
  
                if (i==parentIndex)
                {
                    for (int k = 0,j = 0; j < totalcols; j++)
                    {
                        k = j / colspan;

                        if (k >= curParent.NoOfStates)
                            k = k % (curParent.NoOfStates);

                        if ((i == parentIndex) & (k == stateIndex) )
                        {
                            lstIndexes.Add(j);
                        }
                    }
                }

            }
            return lstIndexes;

        }

        internal void AdjustColumns()
        {
            int i,newColumnCount = 1;
            for (i = 0; i < node.Parents.Count; i++)
            {
                newColumnCount = newColumnCount * ((Node)node.Parents[i]).NoOfStates;
            }

            if (cols < newColumnCount)
            {
                for (i = 0; i < cptTable.Count; i++)
                {
                    for (int j = 0; j < newColumnCount - cols; j++)
                    {
                        cptTable[i].Add(0.0);
                        cptTable[i][cols + j] = 0.5;
                    }
                }
            }
            cols = newColumnCount;
        }
        internal void RemoveColumn(Node parentNode)
        {
            int i, newColumnCount = 1;
            for (i = 0; i < node.Parents.Count; i++)
            {
                if (((Node)node.Parents[i]).NodeID != parentNode.NodeID)
                {
                    newColumnCount = newColumnCount * ((Node)node.Parents[i]).NoOfStates;
                }
            }

            if (cols > newColumnCount)
            {
                for (i = 0; i < cptTable.Count; i++)
                {
                    cptTable[i].RemoveRange(newColumnCount, cols - newColumnCount);
                }
            }
            cols = newColumnCount;
        }

        internal void RemoveColumn(Node parentNode, int stateIndex)
        {
            List<int> colIndexes = GetColumnIndex(node.Parents.IndexOf(parentNode), stateIndex);
            for (int r = 0; r < Rows; r++)
            {
                for (int c = colIndexes.Count-1; c >=0; c--)
                {
                    cptTable[r].RemoveAt(colIndexes[c]);
                }
            }
            cols = cols - colIndexes.Count;
        }

        public List<string> GetAllValues(int rows, int cols)
        {

            int coldef = 2;

            if (cols != 1)
            {
                coldef = cols;
            }

            string[] probabs = new string[cols * rows + (rows * cols)];

            int curIndex = 0;

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    probabs[curIndex++] = GetValue(j, i).ToString();
                    probabs[curIndex++] = " ";
                }
            }
            return probabs.ToList();
        }
    }
}
