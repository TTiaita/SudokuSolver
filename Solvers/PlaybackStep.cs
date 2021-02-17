using Sudoku.Interfaces;
using System.Windows.Media;

namespace Sudoku.Solvers
{
    public class PlaybackStep : IPlaybackStep
    {
        public Brush TextColour { get; set; }
        public Brush BackgroundColour {
            get {
                Brush b = null;
                switch(ActionType)
                {
                    case IPlaybackStep.PlaybackAction.Add:
                        b = Brushes.Blue;
                        break;
                    case IPlaybackStep.PlaybackAction.Try:
                        b = Brushes.Yellow;
                        break;
                    case IPlaybackStep.PlaybackAction.Remove:
                        b = Brushes.Red;
                        break;
                }
                return b;
            }
        }
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IPlaybackStep.PlaybackAction ActionType { get; set; }
    }
}
