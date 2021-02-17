using Microsoft.Win32;
using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sudoku
{
    public partial class SudokuWindow
    {
        private async void LoadBtn_Click(object sender, RoutedEventArgs e) => await Controller.LoadSudokuFile();

        private async void SolveBtn_Click(object sender, RoutedEventArgs e)
        {
            //*
            DisableUI();
            try
            {
                await Controller.SolveSudoku(false);
            }
            catch (Exception ex)
            {
                await ConsoleWriteLine($"{ex.GetType()} occurred.\n\t{ex.Message}");
            }
            finally 
            {
                EnableUI();
            }
            //*/
            await ConsoleWriteLine("Test done");
        }

        private async void PlaybackBtn_Click(object sender, RoutedEventArgs e) => await Controller.SolveSudoku(true);

        private async Task TestAsync()
        {
            //await Task.Run(() =>
            //{
                for (var i = 0; i < 10000; i++)
                {
                    Trace.WriteLine(i);
                }
            //});
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e) => ConsoleText.Text = string.Empty;

        public string AskForFileLoad()
        {
            var diag = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Comma-Seperated Values (*.csv)|*.csv",
                ReadOnlyChecked = true,
                Title = "Open Sudoku file...",
            };
            diag.ShowDialog();
            return diag.FileName;
        }

        public string AskForFileSave()
        {
            var diag = new SaveFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Text File (*.txt)|*.txt",
                Title = "Save log file...",
            };
            diag.ShowDialog();
            return diag.FileName;
        }

        public async Task DrawGameGrid(Solvers.Node[][] gameData)
        {
            GameGrid.Children.Clear();
            GameGrid.ColumnDefinitions.Clear();
            GameGrid.RowDefinitions.Clear();

            for (var i = 0; i < gameData.Length; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            await UpdateGameGrid(gameData);
        }

        public async Task UpdateGameGrid(Solvers.Node[][] gameData)
        {
            var size = gameData.Length;
            var squareSize = (int)Math.Sqrt(size);
            GameGrid.Children.Clear();
            Trace.WriteLine("\nDraw");
            for (var y = 0; y < size; y++)
            {
                var squareRow = (int)Math.Floor((decimal)y / squareSize);
                for (var x = 0; x < size; x++)
                {
                    var node = gameData[x][y];
                    Trace.Write(node.Value + " ");
                    var squareId = node.Z;
                    var border = new Border()
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                    };
                    var cell = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = node.Value == 0 ? " " : node.Value.ToString(),
                        FontSize = 20,
                        Background = Brushes.Transparent,
                        Foreground = node.Starting ? Brushes.Black : Brushes.DarkGreen,
                        TextDecorations = node.Starting ? TextDecorations.Underline : null,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                    };
                    border.Child = cell;

                    Brush colour;
                    if ((squareSize % 2 == 0 && (squareRow % 2 != squareId % 2))
                        || (squareSize % 2 == 1 && squareId % 2 == 0))
                    {
                        colour = Brushes.LightGray;
                    }
                    else
                    {
                        colour = Brushes.WhiteSmoke;
                    }

                    var bg = new Rectangle()
                    {
                        Fill = colour
                    };


                    GameGrid.Children.Add(bg);
                    GameGrid.Children.Add(border);
                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);
                    Grid.SetColumn(bg, x);
                    Grid.SetRow(bg, y);
                }
                Trace.WriteLine("");
            }
        }

        public async Task ConsoleWriteLine(string content)
        {
            ConsoleText.Text += content.Trim() + "\n";
            ConsoleScroll.ScrollToEnd();
            ConsoleScroll.ScrollToLeftEnd();
        }

        public async Task<string> GetAlgortihm()
        {
            return AlgorithmCBox.Text;
        }

        private void DisableUI()
        {
            AlgorithmCBox.IsEnabled = false;
            LoadBtn.IsEnabled = false;
            CheckBtn.IsEnabled = false;
            SolveBtn.IsEnabled = false;
            PlaybackBtn.IsEnabled = false;
            ClearBtn.IsEnabled = false;
            SaveBtn.IsEnabled = false;
        }

        private void EnableUI()
        {
            AlgorithmCBox.IsEnabled = true;
            LoadBtn.IsEnabled = true;
            CheckBtn.IsEnabled = true;
            SolveBtn.IsEnabled = true;
            PlaybackBtn.IsEnabled = true;
            ClearBtn.IsEnabled = true;
            SaveBtn.IsEnabled = true;
        }

        public async Task Playback(Queue<IPlaybackStep> playback)
        {
            throw new NotImplementedException();
        }
    }
}
