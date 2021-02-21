using Sudoku.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class Node : INode
    {
        public bool Starting { get; set; }
        public int Value { get; set; }
        /// <summary>Row Number</summary>
        public int X { get; set; }
        /// <summary>Column Number</summary>
        public int Y { get; set; }
        /// <summary>Box Number</summary>
        public int Z { get; set; }

        public static async Task<Node[][]> FromIntArrayAsync(int[][] data)
        {
            var size = data.Length;
            var squareSize = (int)Math.Sqrt(size);
            var output = new Node[size][];

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    output[x] ??= new Node[size];
                    output[x][y] = new Node()
                    {
                        Starting = data[x][y] != 0,
                        Value = data[x][y],
                        X = x,
                        Y = y,
                        Z = (int)(Math.Floor(y / (decimal)squareSize) * squareSize + Math.Floor(x / (decimal)squareSize))
                    };
                }
            }
            return output;
        }
    }
}
