using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class OptimisedBruteForce : ISolver
    {
        public bool Ready { get; set; }
        protected long steps;
        protected bool playbackEnabled;
        protected OptimisedNodeGroup[] rows;
        protected OptimisedNodeGroup[] cols;
        protected OptimisedNodeGroup[] sqrs;
        protected int size;
        protected OptimisedNode[][] workingData;
        protected Queue<IPlaybackStep> playbackData;
        protected Stopwatch timerInit;
        protected Stopwatch timerSolve;

        public OptimisedBruteForce()
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

            steps = 0;
            size = rawGrid.Length;
            workingData = new OptimisedNode[size][];
            rows = new OptimisedNodeGroup[size];
            cols = new OptimisedNodeGroup[size];
            sqrs = new OptimisedNodeGroup[size];

            for (var i = 0; i < size; i++)
            {
                workingData[i] = new OptimisedNode[size];
                rows[i] = new OptimisedNodeGroup(size) { Id = i};
                cols[i] = new OptimisedNodeGroup(size) { Id = i };
                sqrs[i] = new OptimisedNodeGroup(size) { Id = i };
            }

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var rawCell = rawGrid[x][y];
                    var cell = new OptimisedNode()
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
                    workingData[x][y] = cell;

                    rows[y].AddNode(cell);
                    cols[x].AddNode(cell);
                    sqrs[cell.Z].AddNode(cell);
                }
            }
            playbackData = new Queue<IPlaybackStep>();

            timerInit.Stop();
            Ready = true;
        }

        public async Task<ISolution> Solve(bool enablePlayback)
        {
            if (!Ready)
            {
                throw new InvalidOperationException("Solve() cannot be called befgore Init().");
            }
            timerSolve.Start();

            playbackEnabled = enablePlayback;
            var solved = await RecursiveSolve(0, 0);

            timerSolve.Stop();
            return new Solution()
            {
                Solved = solved,
                Grid = workingData,
                Playback = playbackData,
                TimeToInit = timerInit.ElapsedMilliseconds,
                TimeToSolve = timerSolve.ElapsedMilliseconds,
                TimeTotal = timerInit.ElapsedMilliseconds + timerSolve.ElapsedMilliseconds,
                TotalSteps = steps
            };
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

        protected async Task<bool> RecursiveSolve(int x, int y)
        {
            if (y == size)
            {
                return true;
            }
            var cell = workingData[x][y];
            
            if (cell.Starting)
            {
                AddHistory(cell, IPlaybackStep.PlaybackAction.Add);
                return await RecursiveSolve(cell.NextX, cell.NextY);
            }

            for (var val = 1; val <= size; val++)
            {
                AddHistory(cell, IPlaybackStep.PlaybackAction.Try, val);

                if (cell.IsAllowed(val))
                {
                    cell.Value = val;
                    cell.Blacklist(val);
                    AddHistory(cell, IPlaybackStep.PlaybackAction.Add);
                    if (await RecursiveSolve(cell.NextX, cell.NextY))
                    {
                        return true;
                    }
                    cell.Value = 0;
                    cell.Whitelist(val);

                    AddHistory(cell, IPlaybackStep.PlaybackAction.Remove);
                }
            }

            AddHistory(cell, IPlaybackStep.PlaybackAction.Remove);
            return false;
        }
    }
}