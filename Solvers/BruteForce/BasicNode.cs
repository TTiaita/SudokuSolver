using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class BasicNode : INode
    {
        public bool Starting { get; set; }
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static async Task<BasicNode[][]> FromIntArrayAsync(int[][] data)
        {
            var size = data.Length;
            var squareSize = (int)Math.Sqrt(size);
            var output = new BasicNode[size][];

            Trace.WriteLine("\nFromIntArrayAsync");
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    Trace.Write(data[x][y] + " ");
                    //*
                    output[x] ??= new BasicNode[size];
                    output[x][y] = new BasicNode() 
                    { 
                        Starting = data[x][y] != 0,
                        Value = data[x][y],
                        X = x,
                        Y = y,
                        Z = (int)(Math.Floor(y / (decimal)squareSize) * squareSize + Math.Floor(x / (decimal)squareSize))
                    };
                    //*/
                }
                Trace.WriteLine("");
            }
            return output;
        }
    }
}
