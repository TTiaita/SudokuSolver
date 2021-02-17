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
        private OptimisedNodeGroup[] rows;
        private OptimisedNodeGroup[] cols;
        private OptimisedNodeGroup[] sqrs;
        private int size;
        private OptimisedNode[][] workingData;
        private Queue<IPlaybackStep> playbackData;
        private Stopwatch timerInit;
        private Stopwatch timerSolve;

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

            size = rawGrid.Length;
            workingData = new OptimisedNode[size][];
            rows = new OptimisedNodeGroup[size];
            cols = new OptimisedNodeGroup[size];
            sqrs = new OptimisedNodeGroup[size];

            for (var i = 0; i < size; i++)
            {
                workingData[i] = new OptimisedNode[size];
                rows[i] = new OptimisedNodeGroup(size);
                cols[i] = new OptimisedNodeGroup(size);
                sqrs[i] = new OptimisedNodeGroup(size);
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
                    cols[y].AddNode(cell);
                    sqrs[cell.Z].AddNode(cell);
                }
            }

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

            var solved = await RecursiveSolve(0, 0, enablePlayback);

            timerSolve.Stop();
            return new Solution()
            {
                Solved = solved,
                Grid = workingData,
                Playback = null,
                TimeToInit = timerInit.ElapsedMilliseconds,
                TimeToSolve = timerSolve.ElapsedMilliseconds,
                TimeTotal = timerInit.ElapsedMilliseconds + timerSolve.ElapsedMilliseconds
            };
        }

        private async Task<bool> RecursiveSolve(int x, int y, bool playback)
        {
            var cell = workingData[x][y];

            if (cell.Starting)
            {
                return await RecursiveSolve(cell.NextX, cell.NextY, playback);
            }

            for (var val = 1; val <= size; val++)
            {
                if (IsValid(cell, val))
                {
                    cell.Value = val;
                    cell.Blacklist(val);
                    if (await RecursiveSolve(cell.NextX, cell.NextY, playback))
                    {
                        return true;
                    }
                    cell.Value = 0;
                    cell.Whitelist(val);
                }
            }
            return false;
        }

        private bool IsValid(OptimisedNode node, int val) => node.Row.IsAllowed(val) && node.Col.IsAllowed(val) && node.Sqr.IsAllowed(val);
    }
}