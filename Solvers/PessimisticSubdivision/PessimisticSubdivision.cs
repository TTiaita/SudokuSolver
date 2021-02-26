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
        protected bool playbackEnabled;
        protected long steps;

        protected GraphGroup[] rows;
        protected GraphGroup[] cols;
        protected GraphGroup[] sqrs;

        protected int size;
        protected int squareSize;
        protected GraphNode[][] graph;
        protected Queue<IPlaybackStep> playbackData;
        protected Stopwatch timerInit;
        protected Stopwatch timerSolve;

        protected Fibonacci fibonacci;
        protected GraphNode lastNode;

        public PessimisticSubdivision()
        {
            timerInit = new Stopwatch();
            timerSolve = new Stopwatch();
            Ready = false;
        }

        public async Task Init(INode[][] rawGrid)
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

            steps = 0;
            size = rawGrid.Length;
            squareSize = (int)Math.Sqrt(size);
            graph = new GraphNode[size][];
            rows = new GraphGroup[size];
            cols = new GraphGroup[size];
            sqrs = new GraphGroup[size];

            for (var i = 0; i < size; i++)
            {
                graph[i] = new GraphNode[size];
                rows[i] = new GraphGroup(size, fibonacci) { Id = i };
                cols[i] = new GraphGroup(size, fibonacci) { Id = i };
                sqrs[i] = new GraphGroup(size, fibonacci) { Id = i };
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
                        Starting = rawCell.Starting,
                        Row = rows[y],
                        Col = cols[x],
                        Sqr = sqrs[rawCell.Z],
                    };
                    graph[x][y] = cell;

                    rows[y].AddNode(cell);
                    cols[x].AddNode(cell);
                    sqrs[cell.Z].AddNode(cell);
                }
            }

            playbackData = new Queue<IPlaybackStep>();
            Ready = true;

            timerInit.Stop();
        }

        public async Task<ISolution> Solve(bool enablePlayback)
        {
            if (!Ready)
            {
                throw new InvalidOperationException("Solve() cannot be called befgore Init().");
            }
            timerSolve.Start();

            playbackEnabled = enablePlayback;
            var solved = await RecursiveSolve();

            timerSolve.Stop();
            return new Solution()
            {
                Solved = solved,
                Grid = graph,
                Playback = playbackData,
                TimeToInit = timerInit.ElapsedMilliseconds,
                TimeToSolve = timerSolve.ElapsedMilliseconds,
                TimeTotal = timerInit.ElapsedMilliseconds + timerSolve.ElapsedMilliseconds,
                TotalSteps = steps,
            };
        }

        protected async Task<bool> RecursiveSolve()
        {
            var node = await GetNext();
            if (node == null)
            {
                // Grid is complete
                return true;
            }

            for(var val = 1; val <= size; val++)
            {
                AddHistory(node, IPlaybackStep.PlaybackAction.Try, val);

                if (node.IsAllowed(val))
                {
                    node.Value = val;
                    node.Set(val, lastNode);
                    lastNode = node;

                    AddHistory(node, IPlaybackStep.PlaybackAction.Add);

                    if (await RecursiveSolve())
                    {
                        return true;
                    }
                    else
                    {
                        node.Unset();
                        AddHistory(node, IPlaybackStep.PlaybackAction.Remove);
                    }
                }
            }

            node.Unset();
            lastNode.Unset();
            AddHistory(node, IPlaybackStep.PlaybackAction.Remove);

            return false;
        }

        protected void AddHistory(INode node, IPlaybackStep.PlaybackAction action, int? val = null)
        {
            if (playbackEnabled)
            {
                playbackData.Enqueue(new PlaybackStep()
                {
                    ActionType = action,
                    X = node.X,
                    Y = node.Y,
                    Value = val ?? node.Value
                });
            }
            steps++;
        }

        protected async Task<GraphNode> GetNext()
        {
            // Incomplete square with highest rank
            var square = sqrs
                .Where(a => a.Filled != size)
                .OrderBy(a => a.Rank)
                .LastOrDefault();

            // If all squares are complete then all cells are filled
            if (square == null)
            {
                return null; 
            }

            // Within square highest ranked unfilled cell
            return square.Nodes
                .Where(a => a.Value == 0)
                .OrderBy(a => a.Rank)
                .LastOrDefault();
        }
    }
}