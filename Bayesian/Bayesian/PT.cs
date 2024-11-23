using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBAyes.Bayesian
{
    public class PT
    {
        protected int rows, cols;
        protected List <List<double>> cptTable;
        protected Node node;

        internal PT(Node curNode)
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
    }
}
