using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneAge.Core.Models
{
    public struct Tool
    {
        public static Tool None { get { return new Tool(0); } }
        public static Tool Plus1 { get { return new Tool(1); } }
        public static Tool Plus2 { get { return new Tool(2); } }
        public static Tool Plus3 { get { return new Tool(3); } }
        public static Tool Plus4 { get { return new Tool(4); } }

        public bool Used;
        public readonly int Value;

        private Tool(int value)
        {
            Value = value;
            Used = false;
        }
    }
}
