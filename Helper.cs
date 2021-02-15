using System;
using System.Collections;
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

        public static string MillisecondsToDesc(long ms)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}m {1:D2}s {2:D3}ms", (int)Math.Floor(ts.TotalMinutes), ts.Seconds, ts.Milliseconds);
        }

        public static bool IsValidSudoku(Solvers.Node[][] data) => IsSquareNumber(data.Length) || data.All(a => a.Length == data.Length);
    }
}
