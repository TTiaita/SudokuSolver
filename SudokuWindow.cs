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
using System.Windows.Threading;

namespace Sudoku
{
    public partial class SudokuWindow
    {
        private static bool gridActive = false;
        private INode[][] gameData;
        private static TextBlock[][] gridCells;
        private static int gridSize;
        private static int squareSize;

        private DispatcherTimer frameTick;
        public Queue<IPlaybackStep> PlaybackData { get; set; }
        private IPlaybackStep prevStep;

        public SudokuWindow()
        {
            InitializeComponent();
            frameTick = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(24)
            };
            frameTick.Tick += FrameTick_Tick;

            DisableUI();
            EnableUI();
        }

        private async void FrameTick_Tick(object sender, EventArgs e)
        {
            if (PlaybackData != null && PlaybackData.Count > 0)
            {
                await UpdateGameGrid(PlaybackData.Dequeue());
            }
            else
            {
                PlaybackData = null;
                frameTick.Stop();
                EnableUI();
            }
        }

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
            //try
            //{
                await ClearGameGrid();
                await Controller.SolveSudoku(false);
            /*}
            catch (Exception ex)
            {
                Controller.LogMessage($"{ex.GetType()} occurred.\n\t{ex.Message}");
            }
            finally 
            {//*/
                EnableUI();
            //}
        }

        private async void PlaybackBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            try
            {
                await ClearGameGrid();
                await Controller.SolveSudoku(true);
                StopBtn.IsEnabled = true;
                frameTick.Start();

            }
            catch (Exception ex)
            {
                Controller.LogMessage($"{ex.GetType()} occurred.\n\t{ex.Message}");
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

        private async void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (frameTick.IsEnabled)
            {
                frameTick.Stop();
            }
            await ClearGameGrid();
            EnableUI();
            StopBtn.IsEnabled = false;
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

        public async Task CreateGameGrid(INode[][] newData)
        {
            var size = newData.Length;
            if (!gridActive || size != gridCells.Length)
            {
                gridActive = false;
                await SetupGameGrid(size);
            }
            gameData = newData;
            await UpdateGameGrid();
        }

        private async Task ClearGameGrid()
        {
            if (gridActive)
            {
                // Updates cell contents.
                for (var y = 0; y < gridSize; y++)
                {
                    for (var x = 0; x < gridSize; x++)
                    {
                        var node = gameData[x][y];
                        var cell = gridCells[x][y];
                        node.Value = node.Starting ? node.Value : 0;

                        cell.Text = node.Value == 0 ? " " : node.Value.ToString();
                        cell.Foreground = node.Starting ? Brushes.Black : Brushes.DarkGreen;
                        cell.TextDecorations = node.Starting ? TextDecorations.Underline : null;
                        SetCellBackground(node);
                    }
                }
            }
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

        public async Task UpdateGameGrid()
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

        public async Task UpdateGameGrid(INode[][] data)
        {
            gameData = data;
            await UpdateGameGrid();
        }

        public async Task UpdateGameGrid(IPlaybackStep step)
        {
            if (!gridActive)
            {
                throw new InvalidOperationException("Cannot call UpdateGameGrid() before SetupGameGrid().");
            }

            var node = gameData[step.X][step.Y];
            var cell = gridCells[step.X][step.Y];

            node.Value = step.Value;
            cell.Parent.SetValue(BackgroundProperty, step.BackgroundColour);
            cell.Text = node.Value == 0 ? " " : node.Value.ToString();
            cell.Foreground = step.TextColour;

            if (prevStep != null && (step.X != prevStep.X || step.Y != prevStep.Y))
            {
                node = gameData[prevStep.X][prevStep.Y];
                cell = gridCells[prevStep.X][prevStep.Y];

                cell.Foreground = node.Starting ? Brushes.Black : Brushes.DarkGreen;
                cell.TextDecorations = node.Starting ? TextDecorations.Underline : null;
                SetCellBackground(node);
            }
            prevStep = step;
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
            if (!frameTick.IsEnabled)
            {
                StopBtn.IsEnabled = false;
            }
            SolveBtn.IsEnabled = false;
            PlaybackBtn.IsEnabled = false;
            ClearBtn.IsEnabled = false;
            SaveBtn.IsEnabled = false;
        }

        private void EnableUI()
        {
            AlgorithmCBox.IsEnabled = true;
            LoadBtn.IsEnabled = true;
            if (frameTick.IsEnabled)
            {
                StopBtn.IsEnabled = true;
            }
            if (gridActive)
            {
                SolveBtn.IsEnabled = true;
                PlaybackBtn.IsEnabled = true;
            }
            ClearBtn.IsEnabled = true;
            SaveBtn.IsEnabled = true;
        }

        private void SetCellBackground(INode node)
        {
            Brush colour;
            if ((squareSize % 2 == 0 && (node.Z / squareSize % 2 != node.Z % 2)) || (squareSize % 2 == 1 && node.Z % 2 == 0))
            {
                colour = Brushes.LightGray;
            }
            else
            {
                colour = Brushes.WhiteSmoke;
            }
            gridCells[node.X][node.Y].Parent.SetValue(BackgroundProperty, colour);
        }

    }
}
