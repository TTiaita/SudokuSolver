using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class GraphNode : INode
    {
        public bool Starting { get; set; }
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public GraphGroup Row { get; set; }
        public GraphGroup Col { get; set; }
        public GraphGroup Sqr { get; set; }
        public GraphNode Parent { get; private set; }

        public int Rank
        {
            get
            {
                return Col.Rank + Row.Rank + Sqr.Rank;
            }
        }

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

        public void Set(int val, GraphNode parent)
        {
            if (Value == 0)
            {
                return;
            }

            Blacklist(val);
            Value = val;
            Parent = parent;
        }

        public void Unset()
        {
            if (Value == 0)
            {
                return;
            }

            Whitelist(Value);
            Value = 0;
            Parent = null;
        }

        public bool IsAllowed(int val) => Sqr.IsAllowed(val) && Row.IsAllowed(val) && Col.IsAllowed(val);

        public static async Task<GraphNode[][]> FromIntArrayAsync(int[][] data)
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
                        Z = (int)(Math.Floor(y / (decimal)squareSize) * squareSize + Math.Floor(x / (decimal)squareSize))
                    };
                }
            }
            return output;
        }
    }
}
