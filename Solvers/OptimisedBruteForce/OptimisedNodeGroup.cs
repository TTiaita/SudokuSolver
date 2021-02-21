using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class OptimisedNodeGroup
    {
        public int Size { get; set; }
        public List<OptimisedNode> Nodes { get; set; }
        public bool[] AllowedValues { get; set; }
        public int Id { get; set; }
        protected bool[] mutable;

        public OptimisedNodeGroup(int size)
        {
            Size = size;
            AllowedValues = Enumerable.Repeat(true, size).ToArray();
            mutable = Enumerable.Repeat(true, size).ToArray();
            Nodes = new List<OptimisedNode>();
        }

        public virtual void AddNode(OptimisedNode node)
        {
            Nodes.Add(node);
            if (node.Value != 0)
            {
                Blacklist(node.Value);
                mutable[node.Value - 1] = false;
            }
        }

        public virtual bool IsAllowed(int val) => AllowedValues[val -1];

        public virtual void Blacklist(int val) => AllowedValues[val -1] = false;

        public virtual void Whitelist(int val) => AllowedValues[val -1] = mutable[val -1];
    }
}
