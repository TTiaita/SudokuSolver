using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class Helper
    {
        public static bool IsSquareNumber(int num)
        {
            var sqrt = Math.Sqrt(num);
            return Math.Abs(Math.Ceiling(sqrt) - Math.Floor(sqrt)) < Double.Epsilon;
        }
    }
}
