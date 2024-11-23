using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagramDesigner.Bayesian
{
    public class CPT
    {
        int rows, cols;
        public List <List<double>> cptTable;
        Node node;

        internal CPT(Node curNode)
        {
            cptTable=new List<List<double>>();
            node = curNode;
            rows = 0;
            cols = 1;
        }

        public int Rows 
        {
            get { return cptTable.Count; }
        }

        public int Columns
        {
            get { return cols; }
            set { cols= value; }
        }

        public void SetValue(int row, int col, double value)
        {
            cptTable[row][col] = value;    
        }

        public double GetValue(int row, int col)
        {
            return cptTable[row][col];
        }

        internal void AddRow()
        {   cptTable.Add(new List<double>());
            for (int j = 0; j < cols; j++)
            {
                cptTable[cptTable.Count-1].Add(0.0);
                rows++;
            }
        }
        internal void RemoveRow(int rowIndex)
        {
            cptTable.RemoveAt(rowIndex);
            rows--;
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

                        if ((i == parentIndex) & (k == stateIndex))
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

            if (cols > newColumnCount)
            {
                for (i = 0; i < cptTable.Count; i++)
                {
                    cptTable[i].RemoveRange(newColumnCount, cols - newColumnCount);
                }
            }
            else if (cols < newColumnCount)
            {
                for (i = 0; i < cptTable.Count; i++)
                {
                    for (int j = 0; j < newColumnCount - cols; j++)
                        cptTable[i].Add(0.0);
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
    }
}
