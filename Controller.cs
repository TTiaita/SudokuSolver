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

        private static INode[][] nodeGrid;
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
                nodeGrid = await Solvers.BasicNode.FromIntArrayAsync(rawGrid);

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

            foreach(var col in nodeGrid)
            {
                foreach(var cell in col)
                {
                    cell.Value = cell.Starting ? cell.Value : 0;
                }
            }

            var alg = await MainWindow.GetAlgortihm();
            ISolver solver = alg switch
            {
                "Brute force" => new Solvers.BruteForce(),
                "Optimised brute force" => new Solvers.OptimisedBruteForce(),
                "Pessimistic depth-first" => new Solvers.PessimisticDepthFirst(),
                "Asynchronous subdivision" => new Solvers.AsynchronousSubdivision(),
                _ => throw new ArgumentOutOfRangeException("Specified algorithm could not be found."),
            };

            LogMessage($"Solving with '{alg}' algrithm.");
            await Task.Run(async () =>
            {
                await solver.Init(nodeGrid);
                var solution = await solver.Solve(enablePlayback);
                var timerString = $"\n\tInit Time: { Helper.MillisecondsToDesc(solution.TimeToInit)}\n\tSolve Time: { Helper.MillisecondsToDesc(solution.TimeToSolve)}\n\t----\n\tTotal Time: { Helper.MillisecondsToDesc(solution.TimeTotal)}";

                Application.Current.Dispatcher.Invoke(new Action(async () => {
                    if (solution.Solved && !enablePlayback)
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
                        MainWindow.PlaybackData = solution.Playback;
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
