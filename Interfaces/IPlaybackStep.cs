using System.Windows.Media;

namespace Sudoku.Interfaces
{
    /// <summary>Interface representing one frame in the playback of a solution.</summary>
    public interface IPlaybackStep
    {
        public enum PlaybackAction { Add, Remove };

        public Brush TextColour { get; set; }
        public Brush BackgroundColour { get; set; }
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public PlaybackAction Action { get; set; }
    }
}
