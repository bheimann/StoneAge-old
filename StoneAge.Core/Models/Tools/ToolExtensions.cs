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
