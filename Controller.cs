using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static SudokuWindow MainWindow { get; set; }        
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static string logText = string.Empty;
        public static string LogText
        {
            get { return logText; }
            set
            {
                logText = value;
                OnStaticPropertyChanged("LogText");
            }
        }

        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        private static Solvers.Node[][] nodeGrid;
        private static int[][] rawGrid;

        public static async Task LoadSudokuFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                // User cancelled dialog
                return;
            }

            await Task.Run(async () =>
            {
                LogMessage($"Loading \"{filepath}\"");
                rawGrid = await CSVToArray(filepath);
                nodeGrid = await Solvers.Node.FromIntArrayAsync(rawGrid);

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
            });

            Application.Current.Dispatcher.Invoke(new Action(async () =>
            {
                await MainWindow.CreateGameGrid(nodeGrid);
                LogMessage("Loading complete.");
            }));
        }

        public static async Task SolveSudoku(bool enablePlayback)
        {
            if (nodeGrid == null)
            {
                LogMessage("Sudoku grid must be loaded before solving.");
                return;
            }

            var alg = await MainWindow.GetAlgortihm();
            ISolver solver = alg switch
            {
                "Brute force" => new Solvers.BruteForce(),
                "Optimised brute force" => new Solvers.OptimistedBruteForce(),
                "Pessimistic depth-first" => new Solvers.PessimisticDepthFirst(),
                "Asynchronous subdivision" => new Solvers.AsynchronousSubdivision(),
                _ => throw new ArgumentOutOfRangeException("Specified algorithm could not be found."),
            };

            LogMessage($"Preparing to solve using {alg} algrithm.");
            await Task.Run(async () =>
            {
                await solver.Init(nodeGrid);
                var solution = await solver.Solve(enablePlayback);
                var timerString = $"\n\tInit Time: { Helper.MillisecondsToDesc(solution.TimeToInit)}\n\tSolve Time: { Helper.MillisecondsToDesc(solution.TimeToSolve)}\n\t----\n\tTotal Time: { Helper.MillisecondsToDesc(solution.TimeTotal)}";

                Application.Current.Dispatcher.Invoke(new Action(async () => {
                    if (solution.Solved)
                    {
                        LogMessage($"Solution found.{timerString}");
                        await MainWindow.UpdateGameGrid(solution.Grid);
                    }
                    else
                    {
                        LogMessage($"Failed to find a solution.{timerString}");
                    }

                    if (enablePlayback)
                    {
                    await MainWindow.Playback(solution.Playback);
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
                LogMessage("Invalid CSV file. Cannot create grid.");
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

        public static void LogMessage(string msg)
        {
            LogText += $"{msg}\n";
            Application.Current.Dispatcher.Invoke(new Action(async () =>
            {
                await MainWindow.ConsoleScrollToBottom();
            }));
        }

        public static async Task ClearLog()
        {
            LogText = string.Empty;
        }

        public static async Task SaveLog(string filepath)
        {
            // User cancelled
            if (string.IsNullOrEmpty(filepath))
            {
                return;
            }

            // Write file
            await Task.Run(async () =>
            {
                using var file = new StreamWriter(filepath);
                await file.WriteAsync(LogText);
            });
        }
    }
}
