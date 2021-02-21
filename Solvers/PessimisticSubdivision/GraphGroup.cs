using Sudoku.Heuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class GraphGroup : OptimisedNodeGroup
    {
        public new List<GraphNode> Nodes { get; set; }
        protected static Fibonacci ranker;
        public int Filled { get; set; }
        public int Rank => ranker.Rank(Filled);
        public SortedSet<GraphGroup> SubGroups { get; set; }

        public GraphGroup(int size, Fibonacci fib) : base(size)
        {
            Size = size;
            AllowedValues = Enumerable.Repeat(true, size).ToArray();
            mutable = Enumerable.Repeat(true, size).ToArray();
            Nodes = new List<GraphNode>();
            Filled = Nodes.Count(a => a.Starting);
            ranker = fib;
            SubGroups = new SortedSet<GraphGroup>();
        }

        public override void Blacklist(int val)
        {
            base.Blacklist(val);
            Filled++;
        }

        public override void Whitelist(int val)
        {
            base.Whitelist(val);
            Filled--;
        }

        public GraphNode[] NodesInRange(int start, int end)
        {
            return null;
        }
    }
}
