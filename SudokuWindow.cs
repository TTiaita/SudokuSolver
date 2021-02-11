using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private async void SolveBtn_Click(object sender, RoutedEventArgs e) => await Controller.SolveSudoku();

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ConsoleText.Text = string.Empty;
        }

        public string AskForFile()
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

        public async Task DrawGameGrid(SudokuGrid gameData)
        {
            GameGrid.Children.Clear();
            GameGrid.ColumnDefinitions.Clear();
            GameGrid.RowDefinitions.Clear();

            for (var i = 0; i < gameData.Size; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            await UpdateGameGrid(gameData);
        }

        public async Task UpdateGameGrid(SudokuGrid gameData)
        {
            GameGrid.Children.Clear();
            for (var i = 0; i < gameData.Size; i++)
            {
                for (var ii = 0; ii < gameData.Size; ii++)
                {
                    var startingValue = gameData.Start[i][ii] != 0;
                    var border = new Border()
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                    };
                    var cell = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = gameData.Nodes[i][ii].ProposedValue > 0 ? gameData.Nodes[i][ii].ProposedValue.ToString() : " ",
                        FontSize = 20,
                        Background = Brushes.Transparent,
                        Foreground = startingValue ? Brushes.Black : Brushes.Blue,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0)
                    };
                    border.Child = cell;

                    var bg = new Rectangle()
                    {
                        Fill = gameData.SqrLookup[i][ii] % 2 == 0 ? Brushes.LightGray : Brushes.WhiteSmoke
                    };

                    GameGrid.Children.Add(bg);
                    GameGrid.Children.Add(border);
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, ii);
                    Grid.SetColumn(bg, i);
                    Grid.SetRow(bg, ii);
                }
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
    }
}
