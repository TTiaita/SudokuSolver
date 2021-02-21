using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class GraphNode : OptimisedNode
    {
        public new GraphGroup Row { get; set; }
        public new GraphGroup Col { get; set; }
        public new GraphGroup Sqr { get; set; }

        public int Rank => Col.Rank + Row.Rank + Sqr.Rank;

        public static new async Task<GraphNode[][]> FromIntArrayAsync(int[][] data)
        {
            var size = data.Length;
            var squareSize = (int)Math.Sqrt(size);
            var output = new GraphNode[size][];

            for (var i = 0; i < size; i++)
            {
            }

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    output[x] ??= new GraphNode[size];
                    output[x][y] = new GraphNode()
                    {
                        Starting = data[x][y] != 0,
                        Value = data[x][y],
                        X = x,
                        Y = y,
                        Z = (int)(Math.Floor(y / (decimal)squareSize) * squareSize + Math.Floor(x / (decimal)squareSize)),
                        NextX = x == size - 1 ? 0 : x + 1,
                        NextY = x == size - 1 ? y + 1 : y
                    };
                }
            }
            return output;
        }
    }
}
