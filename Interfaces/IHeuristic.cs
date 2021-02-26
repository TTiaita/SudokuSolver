using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Interfaces
{
    public interface IHeuristic
    {
        public int Rank(int input);
    }
}
