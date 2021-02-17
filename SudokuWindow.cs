using Microsoft.Win32;
using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sudoku
{
    public partial class SudokuWindow
    {
        private static bool gridActive = false;
        private static TextBlock[][] gridCells;
        private static int gridSize;
        private static int squareSize;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Controller.MainWindow = this;
        }

        private async void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            try
            {
                var filepath = AskForFileLoad();
                await Controller.LoadSudokuFile(filepath);
            }
            catch (Exception ex)
            {
                Controller.LogMessage($"{ex.GetType()} occurred.\n\t{ex.Message}");
            }
            finally
            {
                EnableUI();
            }
        }

        private async void SolveBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            try
            {
                await Controller.SolveSudoku(false);
            }
            catch (Exception ex)
            {
                Controller.LogMessage($"{ex.GetType()} occurred.\n\t{ex.Message}");
            }
            finally 
            {
                EnableUI();
            }
        }

        private async void PlaybackBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            try
            {
                await Controller.SolveSudoku(true);
            }
            catch (Exception ex)
            {
                Controller.LogMessage($"{ex.GetType()} occurred.\n\t{ex.Message}");
            }
            finally
            {
                EnableUI();
            }
        }

        private async void ClearBtn_Click(object sender, RoutedEventArgs e) => await Controller.ClearLog();

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            try
            {
                var filepath = AskForFileSave();
                await Controller.SaveLog(filepath);
            }
            catch (Exception ex)
            {
                Controller.LogMessage($"{ex.GetType()} occurred.\n\t{ex.Message}");
            }
            finally
            {
                EnableUI();
            }
        }

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
                CheckPathExists = true,
                Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|All Files (*.*)|*.*",
                Title = "Save log file...",
            };
            diag.ShowDialog();
            return diag.FileName;
        }

        public async Task CreateGameGrid(INode[][] gameData)
        {
            if (!gridActive || gameData.Length != gridCells.Length)
            {
                gridActive = false;
                await SetupGameGrid(gameData.Length);
            }
            await UpdateGameGrid(gameData);
        }

        private async Task SetupGameGrid(int size)
        {
            gridSize = size;
            squareSize = (int)Math.Sqrt(gridSize);
            var converter = TypeDescriptor.GetConverter(typeof(GridLength));

            // Remove existing UI objects
            GameGrid.Children.Clear();

            // Setup Rows
            GameGrid.RowDefinitions.Clear();
            for (var i = 0; i < gridSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = (GridLength)converter.ConvertFromString("*"),
                    MinHeight = 30,
                });
            }

            // Setup columns
            GameGrid.ColumnDefinitions.Clear();
            for (var i = 0; i < gridSize; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = (GridLength)converter.ConvertFromString("*"),
                    MinWidth = 32,
                });
            }

            // Setup array of cell references
            gridCells = new TextBlock[gridSize][];
            for (var i = 0; i < gridSize; i++)
            {
                gridCells[i] = new TextBlock[gridSize];
            }

            // Create the sudoku cells
            for (var y = 0; y < gridSize; y++)
            {
                var squareRow = (int)Math.Floor((decimal)y / squareSize);
                for (var x = 0; x < gridSize; x++)
                {
                    var squareId = Math.Floor(x / (decimal)squareSize) + (Math.Floor(y / (decimal)squareSize) * squareSize);

                    var border = new Border()
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                    };

                    var cell = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 20,
                        Background = Brushes.Transparent,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                    };
                    border.Child = cell;
                    gridCells[x][y] = cell;

                    Brush colour;
                    // Alternate colours between squares
                    if ((squareSize % 2 == 0 && (squareRow % 2 != squareId % 2)) || (squareSize % 2 == 1 && squareId % 2 == 0))
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
            }

            gridActive = true;
        }

        public async Task UpdateGameGrid(INode[][] gameData)
        {
            if (!gridActive)
            {
                throw new InvalidOperationException("Cannot call UpdateGameGrid() before SetupGameGrid().");
            }

            // Updates cell contents and style based on INode[][].
            for (var y = 0; y < gridSize; y++)
            {
                for (var x = 0; x < gridSize; x++)
                {
                    var node = gameData[x][y];
                    var cell = gridCells[x][y];
                    cell.Text = node.Value == 0 ? " " : node.Value.ToString();
                    cell.Foreground = node.Starting ? Brushes.Black : Brushes.DarkGreen;
                    cell.TextDecorations = node.Starting ? TextDecorations.Underline : null;
                }
            }
        }

        public async Task ConsoleScrollToBottom()
        {
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
