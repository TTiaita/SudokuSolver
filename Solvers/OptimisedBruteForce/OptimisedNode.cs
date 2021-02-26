using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class OptimisedNode : Node
    {
        public OptimisedNodeGroup Row { get; set; }
        public OptimisedNodeGroup Col { get; set; }
        public OptimisedNodeGroup Sqr { get; set; }
        public int NextX { get; set; }
        public int NextY { get; set; }

        public void Blacklist(int val)
        {
            Row.Blacklist(val);
            Col.Blacklist(val);
            Sqr.Blacklist(val);
        }

        public void Whitelist(int val)
        {
            Row.Whitelist(val);
            Col.Whitelist(val);
            Sqr.Whitelist(val);
        }

        public virtual bool IsAllowed(int val) => Sqr.IsAllowed(val) && Row.IsAllowed(val) && Col.IsAllowed(val);

        public static new async Task<OptimisedNode[][]> FromIntArrayAsync(int[][] data)
        {
            var size = data.Length;
            var squareSize = (int)Math.Sqrt(size);
            var output = new OptimisedNode[size][];

            for (var i = 0; i < size; i++) { 
            }

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    output[x] ??= new OptimisedNode[size];
                    output[x][y] = new OptimisedNode() 
                    { 
                        Starting = data[x][y] != 0,
                        Value = data[x][y],
                        X = x,
                        Y = y,
                        Z = (int)(Math.Floor(y / (decimal)squareSize) * squareSize + Math.Floor(x / (decimal)squareSize)),
                        NextX = x == size - 1 ? 0: x + 1,
                        NextY = x == size - 1 ? y + 1 : y
                    };
                }
            }
            return output;
        }
    }
}
