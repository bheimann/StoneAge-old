using StoneAge.Core.Models.Tools;
using System.Collections.Generic;

namespace StoneAge.Core.Models.Players
{
    public class PlayerStats
    {
        public string Name { get; private set; }
        public Chair Chair { get; private set; }
        public PlayerColor Color { get; private set; }
        public PlayerMode Mode { get; private set; }
        public bool ReadyToStart { get; private set; }
        public int Food { get; private set; }
        public int FoodTrack { get; private set; }
        public int PeopleToPlace { get; private set; }
        public int TotalPeople { get; private set; }
        public int Wood { get; private set; }
        public int Brick { get; private set; }
        public int Stone { get; private set; }
        public int Gold { get; private set; }
        public IList<Tool> TappedTools { get; private set; }
        public IList<Tool> UntappedTools { get; private set; }
        public int Score { get; private set; }

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
