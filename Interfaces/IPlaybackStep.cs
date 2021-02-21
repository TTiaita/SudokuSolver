using System.Windows.Media;

namespace Sudoku.Interfaces
{
    /// <summary>Interface representing one frame in the playback of a solution.</summary>
    public interface IPlaybackStep
    {
        public enum PlaybackAction { Add, Try, Remove };

        public Brush TextColour { get; }
        public Brush BackgroundColour { get; }
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? PrevX { get; set; }
        public int? PrevY { get; set; }
        public PlaybackAction ActionType { get; set; }
    }
}
