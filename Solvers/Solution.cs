using Sudoku.Interfaces;
using System.Collections.Generic;

namespace Sudoku.Solvers
{
    public class Solution : ISolution
    {
        public bool Solved { get; set; }
        public Queue<IPlaybackStep> Playback { get; set; }
        public INode[][] Grid { get; set; }
        public long TimeToInit { get; set; }
        public long TimeToSolve { get; set; }
        public long TimeTotal { get; set; }
        public long TotalSteps { get; set; }
    }
}
