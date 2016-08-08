namespace StoneAge.Core.Models.BoardSpaces
{
    public interface ISpaceEvaluator
    {
        bool RangeIsValid(int quantity);
        bool HasEnoughFreeSpaces(int usedSpaces, int quantity);
    }
}
