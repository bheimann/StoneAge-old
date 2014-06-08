using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StoneAge.Core;

namespace StoneAge.WinForms
{
    public partial class MainForm : Form
    {
        public GameBoard Game;

        public MainForm()
        {
            InitializeComponent();

            Game = new GameBoard();

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
        }

        private void buttonLOCATION_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Game.Current.Color.ToDrawingColor();
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
    }
}
