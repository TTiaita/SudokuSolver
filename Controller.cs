using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sudoku
{
    public static class Controller
    {
        private static readonly SudokuWindow mainWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as SudokuWindow;
        private static Solvers.Node[][] nodeGrid;
        private static int[][] rawGrid;

        public static async Task LoadSudokuFile()
        {
            var filepath = mainWindow.AskForFileLoad();
            if (string.IsNullOrEmpty(filepath))
            {
                // User cancelled dialog
                return;
            }
            _ = mainWindow.ConsoleWriteLine($"Loading \"{filepath}\"");
            rawGrid = await CSVToArray(filepath);
            nodeGrid = await Solvers.Node.FromIntArrayAsync(rawGrid);

            //*
            Trace.WriteLine("\nNode.Value");
            for (var y = 0; y < nodeGrid.Length; y++)
            {
                for (var x = 0; x < nodeGrid.Length; x++)
                {
                    Trace.Write(nodeGrid[x][y].Value + " ");
                }
                Trace.WriteLine("");
            }

            Trace.WriteLine("\nNode.Square");
            for (var y = 0; y < nodeGrid.Length; y++)
            {
                for (var x = 0; x < nodeGrid.Length; x++)
                {
                    Trace.Write(nodeGrid[x][y].Z + " ");
                }
                Trace.WriteLine("");
            }
            //*/

            await mainWindow.DrawGameGrid(nodeGrid);
            _ = mainWindow.ConsoleWriteLine("Loading complete.");
        }

        public static async Task SolveSudoku(bool enablePlayback)
        {
            if (nodeGrid == null)
            {
                await mainWindow.ConsoleWriteLine("Sudoku grid must be loaded before solving.");
                return;
            }

            var alg = await mainWindow.GetAlgortihm();
            ISolver solver = alg switch
            {
                "Brute force" => new Solvers.BruteForce(),
                "Optimised brute force" => new Solvers.OptimistedBruteForce(),
                "Pessimistic depth-first" => new Solvers.PessimisticDepthFirst(),
                "Asynchronous subdivision" => new Solvers.AsynchronousSubdivision(),
                _ => throw new ArgumentOutOfRangeException("Specified algorithm could not be found."),
            };

            await mainWindow.ConsoleWriteLine($"Preparing to solve using {alg} algrithm.");
            await Task.Run(async () =>
            {
                await solver.Init(nodeGrid);
                var solution = await solver.Solve(enablePlayback);
                var timerString = $"\n\tInit Time: { Helper.MillisecondsToDesc(solution.TimeToInit)}\n\tSolve Time: { Helper.MillisecondsToDesc(solution.TimeToSolve)}\n\t----\n\tTotal Time: { Helper.MillisecondsToDesc(solution.TimeTotal)}";

                Application.Current.Dispatcher.Invoke(new Action(async () => {
                    if (solution.Solved)
                    {
                        await mainWindow.ConsoleWriteLine($"Solution found.{timerString}");
                        await mainWindow.UpdateGameGrid(solution.Grid);
                    }
                    else
                    {
                        await mainWindow.ConsoleWriteLine($"Failed to find a solution.{timerString}");
                    }

                    if (enablePlayback)
                    {
                    await mainWindow.Playback(solution.Playback);
                    }
                }));
            });
        }

        public static async Task<int[][]> CSVToArray(string filepath)
        {
            var rows = File.ReadAllLines(filepath);
            var length = rows.First().Split(',').Length;

            if (!Helper.IsSquareNumber(length))
            {
                // Can't load: Need to do something here
                await mainWindow.ConsoleWriteLine("Invalid CSV file. Cannot create grid.");
                return null;
            }

            var cols = new int[length][];

            Trace.WriteLine("CSVToArray");
            for (var y = 0; y < rows.Length; y++)
            {
                var column = rows[y].Split(',');
                for (var x = 0; x < column.Length; x++)
                {
                    cols[x] ??= new int[length];
                    cols[x][y] = int.Parse(column[x].Trim());

                    Trace.Write(cols[x][y] + " ");
                }
                Trace.WriteLine("");
            }


            return cols;
        }

        
    }
}
