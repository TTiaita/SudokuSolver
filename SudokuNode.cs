using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SudokuNode
    {
        public int Size { get; }
        public int Value { get; set; }
        public int Risk { get; set; }
        public int ProposedValue { get; set; }
        public int Col { get; }
        public int Row { get; }
        public int Sqr { get; }
        public List<int> BlackList { get; private set; }

        public SudokuNode(int size, int value, int col, int row, int sqr)
        {
            Size = size;
            Value = value;
            BlackList = new List<int>();
            Col = col;
            Row = row;
            Sqr = sqr;

            if (value != 0)
            {
                ProposedValue = value;
            }
        }

        public async Task ClearList()
        {
            BlackList = new List<int>();
        }

        public async Task UpdateRiskAsync(int rowRisk, int colRisk, int sqrRisk)
        {
            Risk = rowRisk + colRisk + sqrRisk;
        }
    }
}
