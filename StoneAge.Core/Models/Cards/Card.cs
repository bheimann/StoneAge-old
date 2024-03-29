﻿using System.Collections.Generic;

namespace StoneAge.Core.Models.Cards
{
    [System.Diagnostics.DebuggerDisplay("{CardFinalScoring} {CardImmediate}")]
    public class Card
    {
        // Should be 36 total
        public static readonly IList<Card> All = new List<Card>();

        public static Card GTr1 { get; } = new Card(CardFinalScoring.Transport, CardImmediate.Stone_2);
        public static Card GTr2 { get; } = new Card(CardFinalScoring.Transport, CardImmediate.Lottery);
        public static Card GTi1 { get; } = new Card(CardFinalScoring.Time, CardImmediate.FarmTrack);
        public static Card GTi2 { get; } = new Card(CardFinalScoring.Time, CardImmediate.Lottery);
        public static Card GPo1 { get; } = new Card(CardFinalScoring.Pottery, CardImmediate.Food_7);
        public static Card GPo2 { get; } = new Card(CardFinalScoring.Pottery, CardImmediate.Lottery);
        public static Card GAr1 { get; } = new Card(CardFinalScoring.Art, CardImmediate.Tool_Permanent);
        public static Card GAr2 { get; } = new Card(CardFinalScoring.Art, CardImmediate.Lottery);
        public static Card GHe1 { get; } = new Card(CardFinalScoring.Healing, CardImmediate.Food_5);
        public static Card GHe2 { get; } = new Card(CardFinalScoring.Healing, CardImmediate.Resources_2);
        public static Card GMu1 { get; } = new Card(CardFinalScoring.Music, CardImmediate.Points_3);
        public static Card GMu2 { get; } = new Card(CardFinalScoring.Music, CardImmediate.Points_3);
        public static Card GWr1 { get; } = new Card(CardFinalScoring.Writing, CardImmediate.DrawCard);
        public static Card GWr2 { get; } = new Card(CardFinalScoring.Writing, CardImmediate.Lottery);
        public static Card GWe1 { get; } = new Card(CardFinalScoring.Weaving, CardImmediate.Food_1);
        public static Card GWe2 { get; } = new Card(CardFinalScoring.Weaving, CardImmediate.Food_3);

        public static Card BH11 { get; } = new Card(CardFinalScoring.HutBuilder_1, CardImmediate.Food_4);
        public static Card BH12 { get; } = new Card(CardFinalScoring.HutBuilder_1, CardImmediate.Lottery);
        public static Card BH21 { get; } = new Card(CardFinalScoring.HutBuilder_2, CardImmediate.Food_2);
        public static Card BH22 { get; } = new Card(CardFinalScoring.HutBuilder_2, CardImmediate.Lottery);
        public static Card BH31 { get; } = new Card(CardFinalScoring.HutBuilder_3, CardImmediate.Points_3);

        public static Card BF11 { get; } = new Card(CardFinalScoring.Farmer_1, CardImmediate.Stone_1);
        public static Card BF12 { get; } = new Card(CardFinalScoring.Farmer_1, CardImmediate.FarmTrack);
        public static Card BF13 { get; } = new Card(CardFinalScoring.Farmer_1, CardImmediate.Lottery);
        public static Card BF21 { get; } = new Card(CardFinalScoring.Farmer_2, CardImmediate.Food_3);
        public static Card BF22 { get; } = new Card(CardFinalScoring.Farmer_2, CardImmediate.Lottery);

        public static Card BS11 { get; } = new Card(CardFinalScoring.Shaman_1, CardImmediate.Stone_1);
        public static Card BS12 { get; } = new Card(CardFinalScoring.Shaman_1, CardImmediate.Gold_1);
        public static Card BS13 { get; } = new Card(CardFinalScoring.Shaman_1, CardImmediate.Roll_Stone);
        public static Card BS21 { get; } = new Card(CardFinalScoring.Shaman_2, CardImmediate.Brick_1);
        public static Card BS22 { get; } = new Card(CardFinalScoring.Shaman_2, CardImmediate.Roll_Wood);

        public static Card BT11 { get; } = new Card(CardFinalScoring.ToolMaker_1, CardImmediate.Tool_4);
        public static Card BT12 { get; } = new Card(CardFinalScoring.ToolMaker_1, CardImmediate.Tool_3);
        public static Card BT21 { get; } = new Card(CardFinalScoring.ToolMaker_2, CardImmediate.Tool_2);
        public static Card BT22 { get; } = new Card(CardFinalScoring.ToolMaker_2, CardImmediate.Lottery);
        public static Card BT23 { get; } = new Card(CardFinalScoring.ToolMaker_2, CardImmediate.Lottery);

        private Card(CardFinalScoring cardFinalScoring, CardImmediate cardImmediate)
        {
            CardFinalScoring = cardFinalScoring;
            CardImmediate = cardImmediate;

            All.Add(this);
        }

        public CardImmediate CardImmediate { get; }
        public CardFinalScoring CardFinalScoring { get; }
    }
}
