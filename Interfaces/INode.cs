namespace Sudoku.Interfaces
{
    /// <summary>Interface representing a single node in the sudoku grid</summary>
    public interface INode
    {
        public bool Starting { get; set; }
        public int Value { get; set; }
        /// <summary>Row Number</summary>
        public int X { get; set; }
        /// <summary>Column Number</summary>
        public int Y { get; set; }
        /// <summary>Box Number</summary>
        public int Z { get; set; }
    }
}
