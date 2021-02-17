using System.Collections.Generic;

namespace Sudoku.Interfaces
{
    /// <summary>Interface representing solution to the sudoku grid.</summary>
    public interface ISolution
    {
        public bool Solved { get; set; }
        public Queue<IPlaybackStep> Playback { get; set; }
        public INode[][] Grid { get; set; }
        public long TimeToInit { get; set; }
        public long TimeToSolve { get; set; }
        public long TimeTotal { get; set; }
    }
}
