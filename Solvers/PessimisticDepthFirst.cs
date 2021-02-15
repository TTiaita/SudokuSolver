using Sudoku.Interfaces;
using System.Threading.Tasks;

namespace Sudoku.Solvers
{
    public class PessimisticDepthFirst : ISolver
    {
        public bool Ready { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Task Init(Node[][] rawGrid)
        {
            throw new System.NotImplementedException();
        }

        public Task<ISolution> Solve(bool enablePlayback)
        {
            throw new System.NotImplementedException();
        }
    }
}