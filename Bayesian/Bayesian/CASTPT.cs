using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBAyes.Bayesian
{
    public class CASTPT: PT
    {
        internal CASTPT(Node curNode)
            : base(curNode)
        {
            int i;
            List<int> colIndex = new List<int>(); 

            cols = node.Parents.Count * 2;

             //Adding one more column for Leak
            cols++;

            if (node.NodeType == enmNodeType.NoisyOR)
            {
                AddRow();
                AddRow();
                //for (i = 0; i < node.NoOfStates; i++)
                //{
                //    AddRow();
                //}
            }
            else if (node.NodeType == enmNodeType.CAST)
            {
                AddRow();
            }

        }

        public void AdjustColumns()
        {
            int i, newColumnCount = node.Parents.Count * 2;
            //for (i = 0; i < node.Parents.Count; i++)
            //{
            //    newColumnCount += ((Node)node.Parents[i]).NoOfStates;
            //}
            //newColumnCount++;

            //if (cols > newColumnCount)
            //{
            //    for (i = 0; i < cptTable.Count; i++)
            //    {
            //        cptTable[i].RemoveRange(newColumnCount, cols - newColumnCount);
            //    }
            //}

            //Adjust columns if more states are added
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
        public void AdjustColumns(int parentCount)
        {
            int i, newColumnCount = (parentCount*2)+1;
           
            //Adjust columns if more states are added
              for (i = 0; i < cptTable.Count; i++)
                {
                    for (int j = 0; j < newColumnCount; j++)
                        cptTable[i].Add(0.0);
                }
              cols = newColumnCount;
           
        }

        public void InsertParent(Node parentNode)
        {
            int i,j, colCount=0, newParentIndex;

            newParentIndex = node.Parents.IndexOf(parentNode);

            for (i = 0; i < newParentIndex; i++)
            {
                colCount += ((Node)node.Parents[i]).NoOfStates;
            }
            //if (colCount == 0)
            //    colCount = 0;

            for (i = 0; i < cptTable.Count; i++)
            {
                cptTable[i].Insert(colCount , 0.0);
                cptTable[i].Insert(colCount, 0.0);
            }
            
            cols = cols + 2;
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


        internal int GetStartingIndex(int parentIndex)
        {
            int i, colsBefore = 0;
            for (i = 0; i < parentIndex; i++)
            {
                colsBefore += ((Node)node.Parents[i]).NoOfStates;
            }
            return colsBefore;
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

        public List<string> GetAllNoisyCASTValues(int rows, int col, enmNodeType nodeType)
        {
            string[] probabs = new string[2 * rows * col];

            switch (nodeType)
            {
                case enmNodeType.NoisyOR:
                    {
                        int curIndex = 0;

                        for (int i = 2; i < 4; i++)
                        {
                            for (int j = 0; j <= col; j++)
                            {
                                if (j % 2 != 1)
                                {
                                    probabs[curIndex++] = GetValue(i - 2, j).ToString();
                                    probabs[curIndex++] = " ";
                                }
                            }

                        }
                        break;
                    }
                case enmNodeType.CAST:
                    {
                        int curIndex = 0;

                        for (int i = 2; i < 3; i++)
                        {
                            for (int j = 0; j < col; j++)
                            {
                                probabs[curIndex++] = GetValue(i - 2, j).ToString();
                                probabs[curIndex++] = " ";
                            }

                        }
                        break;
                    }

            }
            return probabs.ToList();

        }



    }
}
