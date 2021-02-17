using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Interfaces
{
    /// <summary>Interface representing an algorithmn that solves the grid.</summary>
    public interface ISolver
    {
        public bool Ready { get; set; }

        public Task Init(INode[][] rawGrid);
        public Task<ISolution> Solve(bool enablePlayback);
    }
}
