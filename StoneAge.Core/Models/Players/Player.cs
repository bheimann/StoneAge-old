using System;

namespace StoneAge.Core.Models.Players
{
    [System.Diagnostics.DebuggerDisplay("{Name} ({Mode}) {Color} {Chair}")]
    public class Player
    {
        public const int MAX_NAME_LENGTH = 100;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public PlayerMode Mode { get; set; }
        public PlayerColor Color { get; set; }
        public Chair Chair { get; set; }
        public PlayerBoard PlayerBoard { get; set; }
        public bool WantsToBeFirstPlayer { get; set; }
        public bool ReadyToStart { get; set; }
        public bool NeedsToFeed { get; set; }
        public BoardSpace? PayingForSpace { get; set; } // TODO: should this go as a GamePhase?

        public Player()
        {
            Id = Guid.NewGuid();
            Name = DefaultPlayerNames[_random.Next(DefaultPlayerNames.Length)];
            Color = PlayerColor.NotChosenYet;
        }

        private static readonly Random _random = new Random();
        public static string[] DefaultPlayerNames { get; } =
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
