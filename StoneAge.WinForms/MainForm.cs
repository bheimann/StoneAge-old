using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StoneAge.Core;
using StoneAge.WinForms.Controls;

namespace StoneAge.WinForms
{
    public partial class MainForm : Form
    {
        public static GameBoard Game;

        public MainForm() : this(new GameBoard())
        {
        }

        public MainForm(GameBoard gameBoard)
        {
            InitializeComponent();

            Game = gameBoard;
            playerDisplay1.SetPlayerBoard(Game.Players[0]);
            playerDisplay2.SetPlayerBoard(Game.Players[1]);
            playerDisplay3.SetPlayerBoard(Game.Players[2]);
            playerDisplay4.SetPlayerBoard(Game.Players[3]);

            UpdateCurrentPlayerName();
            UpdateRemaingResourceCounts();
        }

        private void UpdateCurrentPlayerName()
        {
            labelPlayerNameReplaceable.Text = Game.Current.Name;
            labelPlayerNameReplaceable.ForeColor = Game.Current.Color.ToDrawingColor();
        }

        private void UpdateRemaingResourceCounts()
        {
            labelRemainingWood.Text = Game.Wood.ToString();
            labelRemainingBrick.Text = Game.Brick.ToString();
            labelRemainingStone.Text = Game.Stone.ToString();
            labelRemainingGold.Text = Game.Gold.ToString();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Game.Next();

            UpdateCurrentPlayerName();
            playerDisplay1.RefreshControls();
            playerDisplay2.RefreshControls();
            playerDisplay3.RefreshControls();
            playerDisplay4.RefreshControls();
        }
    }

    public static class ColorExtension
    {
        public static Color ToDrawingColor(this PlayerColor color)
        {
            switch (color)
            {
                case PlayerColor.Green:
                    return Color.Green;
                case PlayerColor.Red:
                    return Color.Red;
                case PlayerColor.Blue:
                    return Color.Blue;
                case PlayerColor.Yellow:
                    return Color.Gold;
                default:
                    throw new Exception("Color not mapped yet!");
            }
        }

        public static Color ToDrawingColor(this PlayerColor? color, Color defaultColor)
        {
            if (color.HasValue)
                return color.Value.ToDrawingColor();
            return defaultColor;
        }
    }
}
