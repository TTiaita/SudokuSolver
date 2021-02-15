using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
    public static class Controller
    {
        private static readonly SudokuWindow mainWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as SudokuWindow;
        private static SudokuGrid activeGrid;

        public static async Task LoadSudokuFile()
        {
            var filepath = mainWindow.AskForFileLoad();
            _ = mainWindow.ConsoleWriteLine($"Loading \"{filepath}\"");
            activeGrid = new SudokuGrid(await CSVToArray(filepath));
            if (activeGrid != null)
            {
                await mainWindow.DrawGameGrid(activeGrid);
            }
            _ = mainWindow.ConsoleWriteLine("Loading complete.");
        }

        public static async Task SolveSudoku()
        {
            if (activeGrid == null)
            {
                await mainWindow.ConsoleWriteLine("Sudoku grid must be loaded before solving.");
                return;
            }
            var alg = await mainWindow.GetAlgortihm();
            await mainWindow.ConsoleWriteLine($"Preparing to solve using {alg} algrithm.");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var success = await activeGrid.Solve();
            stopwatch.Stop();

            if (success)
            {
                await mainWindow.ConsoleWriteLine($"Solution found.\n\tElapsed Time: {Helper.MillisecondsToDesc(stopwatch.ElapsedMilliseconds)}");
            }
            else
            {
                await mainWindow.ConsoleWriteLine($"Failed to find a solution.\n\tElapsed Time: {Helper.MillisecondsToDesc(stopwatch.ElapsedMilliseconds)}");
            }
        }

        public static async Task<int[][]> CSVToArray(string filepath)
        {
            var rows = File.ReadAllLines(filepath);
            var length = rows.First().Split(',').Count();

            if (!Helper.IsSquareNumber(length))
            {
                // Can't load: Need to do something here
                await mainWindow.ConsoleWriteLine("Invalid CSV file. Cannot create grid.");
                return null;
            }

            var cols = new int[length][];

            for (var i = 0; i < rows.Length; i++)
            {
                var cells = rows[i].Split(',');
                for (var ii = 0; ii < cells.Length; ii++)
                {
                    cols[ii] ??= new int[length];
                    cols[ii][i] = int.Parse(cells[ii].Trim());
                }
            }

            return cols;
        }

        
    }
}
