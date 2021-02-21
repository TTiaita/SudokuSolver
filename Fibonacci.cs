using System;

namespace Sudoku
{
    public class Fibonacci
    {
        private int[] sequence;
        private int size;

        public Fibonacci(int length)
        {
            if (length < 4)
            {
                throw new ArgumentException("Length must be equal to or greater than four.");
            }

            size = length;
            sequence = new int[size];
            sequence[0] = 1;
            sequence[1] = 2;
            for (var i = 2; i < size; i++)
            {
                sequence[i] = sequence[i - 1] + sequence[i - 2];
            }
        }
        
        public int Rank(int position)
        {
            if (position > size)
            {
                throw new ArgumentOutOfRangeException($"'position' is greater than the limit of {sequence.Length}.");
            }

            return sequence[position];
        }
    }
}
