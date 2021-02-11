using System;

namespace Sudoku
{
    public static class Fibonacci
    {
        // F1-F25
        private static int[] sequence = { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 1771, 28657, 46368, 75025 };

        public static int Rank(int position)
        {
            if (position > sequence.Length)
            {
                throw new ArgumentOutOfRangeException($"'position' is greater than the limit of {sequence.Length}.");
            }
            return sequence[position];
        }
    }
}
