namespace StoneAge.Core.Models.Players
{
    public class StandardPlayerBoardFactory : IPlayerBoardFactory
    {
        public PlayerBoard CreateNew()
        {
            return new PlayerBoard();
        }
    }
}
