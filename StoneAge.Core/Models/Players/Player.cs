using System;

namespace StoneAge.Core.Models.Players
{
    [System.Diagnostics.DebuggerDisplay("{Name} ({Mode}) {Color} {Chair}")]
    public class Player
    {
        public const int MAX_NAME_LENGTH = 100;

        public Guid Id;
        public string Name;
        public PlayerMode Mode;
        public PlayerColor Color;
        public Chair Chair;
        public PlayerBoard PlayerBoard;
        public bool WantsToBeFirstPlayer;
        public bool ReadyToStart;
        public bool NeedsToFeed;
        public BoardSpace? PayingForSpace; // TODO: should this go as a GamePhase?

        public Player()
        {
            Id = Guid.NewGuid();
            Name = DefaultPlayerNames[_random.Next(DefaultPlayerNames.Length)];
            Color = PlayerColor.NotChosenYet;
        }

        private static readonly Random _random = new Random();
        public static string[] DefaultPlayerNames = new[]
        {
            "Michael",
            "Carol",
            "Scott",
            "Olivia",
            "Griffin",
            "Benjamin",
            "Tiffany",
            "Brooklynn",
            "Shane",
            "Matt",
            "Becky",
            "Oliver",
            "Dave",
            "Mel",
            "Abby",
            "Ash",
        };
    }
}
