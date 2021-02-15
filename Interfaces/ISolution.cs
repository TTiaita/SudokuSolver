using System.Collections.Generic;
using System.Diagnostics;

namespace Sudoku.Interfaces
{
    /// <summary>Interface representing solution to the sudoku grid.</summary>
    public interface ISolution
    {
        public Queue<IPlaybackStep> Playback { get; set; }
        public int[][] Grid { get; set; }
        public Stopwatch Stopwatch { get; set; }
    }
}
