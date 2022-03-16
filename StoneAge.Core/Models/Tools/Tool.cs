namespace StoneAge.Core.Models.Tools
{
    [System.Diagnostics.DebuggerDisplay("{Value == 0 ? \"None\" : \"Plus \" + Value} {Used ? \"Tapped\" : \"Untapped\"}")]
    public struct Tool
    {
        public static Tool None => new Tool(0);
        public static Tool Plus1 => new Tool(1);
        public static Tool Plus2 => new Tool(2);
        public static Tool Plus3 => new Tool(3);
        public static Tool Plus4 => new Tool(4);

        public bool Used { get; set; }
        public readonly int Value;

        private Tool(int value)
        {
            Value = value;
            Used = false;
        }

        public static Tool ByValue(int value)
        {
            return new Tool(value);
        }
    }
}
