using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneAge.Core.Models.Tools
{
    public static class ToolExtensions
    {
        public static Tool Tap(this Tool tool)
        {
            tool.Used = true;
            return tool;
        }
    }
}
