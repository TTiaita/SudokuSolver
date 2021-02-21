using Sudoku.Interfaces;
using System.Windows.Media;

namespace Sudoku.Solvers
{
    public class PlaybackStep : IPlaybackStep
    {
        public Brush TextColour
        {
            get
            {
                Brush b = null;
                switch (ActionType)
                {
                    case IPlaybackStep.PlaybackAction.Add:
                        b = Brushes.Black;
                        break;
                    case IPlaybackStep.PlaybackAction.Try:
                        b = Brushes.Black;
                        break;
                    case IPlaybackStep.PlaybackAction.Remove:
                        b = Brushes.Black;
                        break;
                }
                return b;
            }
        }
        public Brush BackgroundColour {
            get {
                Brush b = null;
                switch(ActionType)
                {
                    case IPlaybackStep.PlaybackAction.Add:
                        b = Brushes.LightBlue;
                        break;
                    case IPlaybackStep.PlaybackAction.Try:
                        b = Brushes.LightYellow;
                        break;
                    case IPlaybackStep.PlaybackAction.Remove:
                        b = Brushes.Salmon;
                        break;
                }
                return b;
            }
        }
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IPlaybackStep.PlaybackAction ActionType { get; set; }
        public int? PrevX { get; set; }
        public int? PrevY { get; set; }
    }
}
