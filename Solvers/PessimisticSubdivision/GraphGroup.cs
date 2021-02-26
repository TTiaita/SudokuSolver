using Sudoku.Heuristics;
using Sudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class GraphGroup
    {

        public int Size { get; set; }
        public bool[] AllowedValues { get; set; }
        public int Id { get; set; }
        protected bool[] mutable;

        public List<GraphNode> Nodes { get; set; }
        public int Filled { get; set; }
        public int Rank => ranker.Rank(Filled);
        protected IHeuristic ranker;

        public GraphGroup(int size, IHeuristic heuristic)
        {
            Size = size;
            AllowedValues = Enumerable.Repeat(true, size).ToArray();
            mutable = Enumerable.Repeat(true, size).ToArray();
            Nodes = new List<GraphNode>();
            Filled = Nodes.Count(a => a.Starting);
            ranker = heuristic;
        }

        public void AddNode(GraphNode node)
        {
            Nodes.Add(node);
            if (node.Value != 0)
            {
                Blacklist(node.Value);
                mutable[node.Value - 1] = false;
            }
        }

        public void Blacklist(int val)
        {
            AllowedValues[val - 1] = false;
            Filled++;
        }

        public void Whitelist(int val)
        {
                AllowedValues[val - 1] = mutable[val - 1];
                Filled--;
        }

        public virtual bool IsAllowed(int val) => AllowedValues[val - 1];

        public GraphNode[] NodesInRange(int start, int end)
        {
            return null;
        }
    }
}
