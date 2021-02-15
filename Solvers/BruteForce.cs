using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Interfaces;

namespace Sudoku.Solvers
{
    public class BruteForce : ISolver
    {
        protected Node[][] data;
        protected int size;

        protected Stopwatch timerInit;
        protected Stopwatch timerSolve;

        public bool Ready { get; set; } = false;

        public BruteForce()
        {
            timerInit = new Stopwatch();
            timerSolve = new Stopwatch();
        }

        public async Task Init(Node[][] rawGrid)
        {
            if (!Helper.IsValidSudoku(rawGrid))
            {
                throw new ArgumentException("Provided data is not a valid sudoku grid.");
            }
            timerInit.Start();

            size = rawGrid.Length;
            data = new Node[size][];
            for (var  i = 0; i < size; i++)
            {
                data[i] = new Node[size];
                for (var ii = 0; ii < size; ii++)
                {
                    data[i][ii] = new Node() {
                        Value = rawGrid[i][ii].Value,
                        Starting = rawGrid[i][ii].Starting,
                        X = rawGrid[i][ii].X,
                        Y = rawGrid[i][ii].Y,
                        Z = rawGrid[i][ii].Z
                    };
                }
            }
            
            timerInit.Stop(); 
            Ready = true;
        }

        public async Task<ISolution> Solve(bool enablePlayback)
        {
            if (!Ready)
            {
                throw new InvalidOperationException("");
            }
            timerSolve.Start();

            var solvedGrid = RecursiveSolve(0, 0, enablePlayback);

            timerSolve.Stop();
            return new Solution()
            {
                Solved = solvedGrid != null,
                Grid = solvedGrid,
                Playback = null,
                TimeToInit = timerInit.ElapsedMilliseconds,
                TimeToSolve = timerSolve.ElapsedMilliseconds,
                TimeTotal = timerInit.ElapsedMilliseconds + timerSolve.ElapsedMilliseconds
            };
        }

        protected Node[][] RecursiveSolve(int x, int y, bool enablePlayback)
        {
            if (y == size)
            {
                return data;
            }
            var cell = data[x][y];
            var nextX = (x + 1) % size;
            var nextY = (x == size - 1) ? y + 1 : y;
            if (!cell.Starting)
            {
                for (var proposed = 1; proposed <= size; proposed++)
                {
                    if (!(GetRowData(x).Contains(proposed) || GetColData(y).Contains(proposed) || GetBoxData(x,y).Contains(proposed)))
                    {
                        cell.Value = proposed;
                        var next = RecursiveSolve(nextX, nextY, enablePlayback);
                        if (next != null)
                        {
                            return next;
                        }
                    }
                    continue;
                }
                return null;
            }
            else
            {
                return RecursiveSolve(nextX, nextY, enablePlayback);
            }
        }

        protected List<int> GetBoxData(int x, int y)
        {
            var z = GetBoxId(x, y);
            var startX = (int)(Math.Floor(z / (decimal)size));
            var startY = z * size;
            var boxData = new List<int>();
            for (var i = startX; i < startX + size; i++)
            {
                for (var ii = startY; ii < startY + size; ii++)
                {
                    boxData.Add(data[x][y].Value);
                }
            }
            return boxData;
        }

        protected List<int> GetRowData(int y)
        {
            var rowData = new List<int>();
            for (var i = 0; i < size; i++)
            {
                rowData.Add(data[y][i].Value);
            }
            return rowData;
        }

        protected List<int> GetColData(int x)
        {
            var colData = new List<int>();
            for (var i = 0; i < size; i++)
            {
                colData.Add(data[i][x].Value);
            }
            return colData;
        }

        protected int GetBoxId(int x, int y)
        {
            return (int)(Math.Floor(x / (decimal)size) * size + Math.Floor(y / (decimal)size));
        }
    }
}
