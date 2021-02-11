using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class Controller
    {
    }

    public static class Helper
    {
        public static bool IsSquareNumber(int num)
        {
            var sqrt = Math.Sqrt(num);
            return Math.Abs(Math.Ceiling(sqrt) - Math.Floor(sqrt)) < Double.Epsilon;
        }
    }

    public class SudokuData
    {
        public int Size { get; }
        public int[][] Start { get; }
        public int[][] Data { get;  }
        public int[] Rows { get; }
        public int[] Columns { get; }
        
        public SudokuData(int[][] startingValues)
        {
            Start = startingValues.Select(a => a.ToArray()).ToArray();
            Data = startingValues.Select(a => a.ToArray()).ToArray();
        }

    }

    public static class Fibonacci
    {
        // F1-F25
        private static int[] sequence = { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 1771, 28657, 46368, 75025 };

        public static int Rank(int position)
        {
            if (position > sequence.Length)
            {
                throw new ArgumentOutOfRangeException($"'position' is greater than the limit of {sequence.Length}.");
            }
            return sequence[position];
        }
    }

    public class SudokuGrid
    {
        // Meta data
        public int Size { get; }
        public int SquareSize { get; }
        public int[][] Start { get; }

        // Node lists
        public SudokuNode[][] Nodes { get; private set; }
        private List<SudokuNode>[] rows;
        private List<SudokuNode>[] cols;
        private List<SudokuNode>[] sqrs;
        public int[][] SqrLookup { get; private set; }

        // Optimisation arrays
        private List<int>[] rowTaboo;
        private List<int>[] colTaboo;
        private List<int>[] sqrTaboo;
        private int[] colRisks;
        private int[] rowRisks;
        private int[] sqrRisks;

        public SudokuGrid(int[][] startingValues)
        {
            // Store starting statre
            Start = startingValues;

            // Determine grid size
            Size = startingValues.Length;
            SquareSize = (int)Math.Sqrt(Size);

            // Init Lists
            Nodes = new SudokuNode[Size][];
            cols = new List<SudokuNode>[Size];
            rows = new List<SudokuNode>[Size];
            sqrs = new List<SudokuNode>[Size];
            SqrLookup = new int[Size][];

            // Init optimisation arrays
            colTaboo = new List<int>[Size];
            rowTaboo = new List<int>[Size];
            sqrTaboo = new List<int>[Size];
            colRisks = new int[Size];
            rowRisks = new int[Size];
            sqrRisks = new int[Size];

            // Create grid structure
            for (var i = 0; i < Size; i++)
            {
                Nodes[i] = new SudokuNode[Size];
                cols[i] = new List<SudokuNode>();
                SqrLookup[i] = new int[Size];

                for (var ii = 0; ii < Size; ii++)
                {
                    // Create Square lookup
                    var sId = (int)(Math.Floor(i / (decimal)SquareSize) * SquareSize + Math.Floor(ii / (decimal)SquareSize));
                    SqrLookup[i][ii] = sId;

                    // Create new node
                    var node = new SudokuNode(Size, Start[i][ii], i, ii, sId);
                    Nodes[i][ii] = node;

                    // Add node to column (x)
                    cols[i].Add(node);

                    // Add node to row (y)
                    rows[ii] ??= new List<SudokuNode>();
                    rows[ii].Add(node);

                    // Add node to square (z)
                    sqrs[sId] ??= new List<SudokuNode>();
                    sqrs[sId].Add(node);
                }
            }
        }

        public int GetSquare(int col, int row) => SqrLookup[col][row];

        public void DebugPrint()
        {
            Trace.WriteLine($"\nSize: {Size}. Square: {SquareSize}.");
            for (var i = 0; i < Size; i++)
            {
                var line = "";
                for (var ii = 0; ii < Size; ii++)
                {
                    line += Nodes[i][ii].Value + " ";
                }
                Trace.WriteLine(line);
            }

            Trace.WriteLine("\nLookup");
            for (var i = 0; i < Size; i++)
            {
                var line = "";
                for (var ii = 0; ii < Size; ii++)
                {
                    line += SqrLookup[i][ii] + " ";
                }
                Trace.WriteLine(line);
            }
        }

        /// <summary>
        /// Checks if the specified value is valid to place in the cell indicated
        /// </summary>
        public async Task<bool> CheckValidAsync(int col, int row, int sqr, int value) => !(colTaboo[col].Contains(value) || rowTaboo[row].Contains(value) || sqrTaboo[sqr].Contains(value));

        protected async Task UpdateNodesAsync()
        {
            // Update lists in parallel
            var updates = new Task[]
            {
                UpdateRowRiskAsync(),
                UpdateColRiskAsync(),
                UpdateSquareRiskAsync(),
                UpdateColTabooAsync(),
                UpdateRowTabooAsync(),
                UpdateSquareTabooAsync(),
            };
            Task.WaitAll(updates);

            await UpdateNodesRiskAsync();
        }

        protected async Task UpdateColRiskAsync()
        {
            for (var i = 0; i < Size; i++)
            {
                colRisks[i] = Fibonacci.Rank(cols[i].Count(a => a.Value != 0));
            }
        }

        protected async Task UpdateRowRiskAsync()
        {
            for (var i = 0; i < Size; i++)
            {
                rowRisks[i] = Fibonacci.Rank(rows[i].Count(a => a.Value != 0));
            }
        }

        protected async Task UpdateSquareRiskAsync()
        {
            for (var i = 0; i < Size; i++)
            {
                sqrRisks[i] = Fibonacci.Rank(sqrs[i].Count(a => a.Value != 0));
            }
        }

        protected async Task UpdateNodesRiskAsync()
        {
            var updates = new List<Task>();
            for (var i = 0; i < Size; i++)
            {
                for (var ii = 0; ii < Size; ii++)
                {
                    if (Nodes[i][ii].Value == 0)
                    {
                        updates.Add(Nodes[i][ii].UpdateRiskAsync(rowRisks[i], colRisks[ii], sqrRisks[SqrLookup[i][ii]]));
                    }
                }
            }
            Task.WaitAll(updates.ToArray());
        }

        protected async Task UpdateColTabooAsync()
        {
            for (var i = 0; i < Size; i++)
            {
                colTaboo[i] = cols[i].Where(a => a.Value > 0).Select(a => a.Value).ToList();
            }
        }

        protected async Task UpdateRowTabooAsync()
        {
            for (var i = 0; i < Size; i++)
            {
                rowTaboo[i] = rows[i].Where(a => a.Value > 0).Select(a => a.Value).ToList();
            }
        }

        protected async Task UpdateSquareTabooAsync()
        {
            for (var i = 0; i < Size; i++)
            {
                sqrTaboo[i] = sqrs[i].Where(a => a.Value > 0).Select(a => a.Value).ToList();
            }
        }

        public int NodeCount() => Size * Size;
    }

    public class SudokuNode
    {
        public int Size { get; }
        public int Value { get; set; }
        public int Risk { get; set;  }
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
