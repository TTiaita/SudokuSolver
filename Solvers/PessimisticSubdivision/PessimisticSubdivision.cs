using Sudoku.Heuristics;
using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace Sudoku.Solvers
{
    public class PessimisticSubdivision : ISolver
    {
        public bool Ready { get; set; }

        protected GraphGroup[] rows;
        protected GraphGroup[] cols;
        protected GraphGroup[] sqrs;
        protected List<GraphGroup> rowsSorted;
        protected List<GraphGroup> colsSorted;
        protected List<GraphGroup> sqrsSorted;
        protected bool[] sqrsSolved;

        protected int size;
        protected int squareSize;
        protected GraphNode[][] graph;
        protected Queue<IPlaybackStep> playbackData;
        protected Stopwatch timerInit;
        protected Stopwatch timerSolve;
        protected Fibonacci fibonacci;

        public PessimisticSubdivision()
        {
            timerInit = new Stopwatch();
            timerSolve = new Stopwatch();
            Ready = false;
        }

        public Task Init(INode[][] rawGrid)
        {
            if (!Helper.IsValidSudoku(rawGrid))
            {
                throw new ArgumentException("Provided data is not a valid sudoku grid.");
            }
            timerInit.Start();

            if (fibonacci == null || rawGrid.Length != fibonacci.Size)
            {
                fibonacci = new Fibonacci(rawGrid.Length);
            }

            size = rawGrid.Length;
            squareSize = (int)Math.Sqrt(size);
            graph = new GraphNode[size][];
            rows = new GraphGroup[size];
            cols = new GraphGroup[size];
            sqrs = new GraphGroup[size];

            rowsSorted = new List<GraphGroup>();
            colsSorted = new List<GraphGroup>();
            sqrsSorted = new List<GraphGroup>();

            for (var i = 0; i < size; i++)
            {
                graph[i] = new GraphNode[size];
                rows[i] = new GraphGroup(size, fibonacci) { Id = i };
                cols[i] = new GraphGroup(size, fibonacci) { Id = i };
                sqrs[i] = new GraphGroup(size, fibonacci) { Id = i };
                rowsSorted.Add(rows[i]);
                colsSorted.Add(cols[i]);
                sqrsSorted.Add(sqrs[i]);
            }

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var rawCell = rawGrid[x][y];
                    var cell = new GraphNode()
                    {
                        X = x,
                        Y = y,
                        Z = rawCell.Z,
                        Value = rawCell.Value,
                        NextX = x == (size - 1) ? 0 : x + 1,
                        NextY = x == (size - 1) ? y + 1 : y,
                        Starting = rawCell.Starting,
                        Row = rows[y],
                        Col = cols[x],
                        Sqr = sqrs[rawCell.Z],
                    };
                    graph[x][y] = cell;
                    sqrs[cell.Z].SubGroups.Add(rows[y]);
                    rows[y].SubGroups.Add(cols[x]);

                    rows[y].AddNode(cell);
                    cols[x].AddNode(cell);
                    sqrs[cell.Z].AddNode(cell);
                }
            }

            playbackData = new Queue<IPlaybackStep>();

            return null;
        }

        public async Task<ISolution> Solve(bool enablePlayback)
        {
            var solved = await RecursiveSolve();
            return null;
        }

        protected async Task<bool> RecursiveSolve()
        {
            var node = GetNext();
            if (node == null)
            {
                // Grid is complete
                return true;
            }


            return false;
        }

        protected async Task<GraphNode> GetNext()
        {
            GraphNode next = null;

            do
            {
                // Incomplete square with highest rank
                var square = sqrsSorted.OrderBy(a => a.Rank).LastOrDefault(a => a.Filled != size);
                if (square == null)
                {
                    return null; 
                }

                var xStart = (square.Id * squareSize) % size;
                var yStart = (square.Id / squareSize) * squareSize;

                // Incomplete row with highest rank in square
                var row = square.SubGroups.OrderBy(a => a.Rank).Last(a => a.Filled != size);

                // Incomplete column with highest rank row
                var col = row.SubGroups.OrderBy(a => a.Rank).Last(a => a.Filled != size);
                next = graph[col.Id][row.Id];
            } while (next == null);

            return next;
        }
    }
}