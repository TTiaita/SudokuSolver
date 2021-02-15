using System.Threading.Tasks;

namespace Sudoku.Interfaces
{
    /// <summary>Interface representing an algorithmn that solves the grid.</summary>
    public interface ISolver
    {
        public Task Init(int[][] rawGrid);
        public Task<ISolution> Solve(bool enablePlayback);
    }
}
