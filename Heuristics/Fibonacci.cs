using Sudoku.Interfaces;
using System;

namespace Sudoku.Heuristics
{
    public class Fibonacci : IHeuristic
    {
        private int[] sequence;
        public int Size { get; private set; }

        public Fibonacci(int length)
        {
            if (length < 4)
            {
                throw new ArgumentException("Length must be equal to or greater than four.");
            }

            Size = length;
            sequence = new int[Size];
            sequence[0] = 1;
            sequence[1] = 2;
            for (var i = 2; i < Size; i++)
            {
                sequence[i] = sequence[i - 1] + sequence[i - 2];
            }
        }
        
        public int Rank(int position)
        {
            if (position > Size)
            {
                throw new ArgumentOutOfRangeException($"'position' is greater than the limit of {sequence.Length}.");
            }

            return sequence[position];
        }
    }
}
