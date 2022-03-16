using StoneAge.Core.Models.Tools;
using System.Collections.Generic;

namespace StoneAge.Core.Models.Players
{
    public class PlayerStats
    {
        public string Name { get; }
        public Chair Chair { get; }
        public PlayerColor Color { get; }
        public PlayerMode Mode { get; }
        public bool ReadyToStart { get; }
        public int Food { get; }
        public int FoodTrack { get; }
        public int PeopleToPlace { get; }
        public int TotalPeople { get; }
        public int Wood { get; }
        public int Brick { get; }
        public int Stone { get; }
        public int Gold { get; }
        public IList<Tool> TappedTools { get; }
        public IList<Tool> UntappedTools { get; }
        public int Score { get; }

        public PlayerStats(Player player)
        {
            Name = player.Name;
            Chair = player.Chair;
            Color = player.Color;
            Mode = player.Mode;
            ReadyToStart = player.ReadyToStart;

            TappedTools = new List<Tool>();
            UntappedTools = new List<Tool>();

            if (player.PlayerBoard == null)
                return;
            Food = player.PlayerBoard.Food;
            FoodTrack = player.PlayerBoard.FoodTrack;
            PeopleToPlace = player.PlayerBoard.PeopleToPlace;
            TotalPeople = player.PlayerBoard.TotalPeople;
            Score = player.PlayerBoard.Score;

            Wood = player.PlayerBoard.Resources[Resource.Wood];
            Brick = player.PlayerBoard.Resources[Resource.Brick];
            Stone = player.PlayerBoard.Resources[Resource.Stone];
            Gold = player.PlayerBoard.Resources[Resource.Gold];

            AddTappedTool(player, 0);
            AddTappedTool(player, 1);
            AddTappedTool(player, 2);
        }

        private void AddTappedTool(Player player, int index)
        {
            var tool = player.PlayerBoard.Tools[index];
            if (tool.Value == 0)
                return;

            if (tool.Used)
                TappedTools.Add(tool);
            else
                UntappedTools.Add(tool);
        }
    }
}
