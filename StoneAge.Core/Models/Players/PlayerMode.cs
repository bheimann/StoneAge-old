namespace StoneAge.Core.Models.Players
{
    public enum PlayerMode
    {
        HumanLocal,
        HumanInternet,
        ComputerStrategyRandom,
        ComputerStrategyPeople,
        ComputerStrategyFarm,
        ComputerStrategyTool,
        ComputerStrategyCard,
        ComputerStrategyHuts,
        ComputerStrategyGreedyBlock,
        ComputerEasy,// not sure what this means
        ComputerHard,// not sure what this means
        ComputerUnknown,// chooses a strategy at random
    }
}
