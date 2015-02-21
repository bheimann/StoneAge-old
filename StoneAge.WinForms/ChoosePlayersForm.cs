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
    public partial class ChoosePlayersForm : Form
    {
        public ChoosePlayersForm() : this(new GameBoard())
        {
        }

        public ChoosePlayersForm(GameBoard gameBoard)
        {
            InitializeComponent();

            _game = gameBoard;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            _game.Players[0] = new PlayerBoard(textBoxName1.Text, GetPlayerColor(radioButtonBlue1, radioButtonGreen1, radioButtonRed1, radioButtonYellow1, radioButtonNone1));
            _game.Players[1] = new PlayerBoard(textBoxName2.Text, GetPlayerColor(radioButtonBlue2, radioButtonGreen2, radioButtonRed2, radioButtonYellow2, radioButtonNone2));
            _game.Players[2] = new PlayerBoard(textBoxName3.Text, GetPlayerColor(radioButtonBlue3, radioButtonGreen3, radioButtonRed3, radioButtonYellow3, radioButtonNone3));
            _game.Players[3] = new PlayerBoard(textBoxName4.Text, GetPlayerColor(radioButtonBlue4, radioButtonGreen4, radioButtonRed4, radioButtonYellow4, radioButtonNone4));

            if (radioButtonNone1.Checked)
                _game.Players[0].DroppedOut = true;
            if (radioButtonNone2.Checked)
                _game.Players[1].DroppedOut = true;
            if (radioButtonNone3.Checked)
                _game.Players[2].DroppedOut = true;
            if (radioButtonNone4.Checked)
                _game.Players[3].DroppedOut = true;

            this.Close();
        }

        private PlayerColor GetPlayerColor(RadioButton blue, RadioButton green, RadioButton red, RadioButton yellow, RadioButton none)
        {
            if (blue.Checked)
                return PlayerColor.Blue;
            if (green.Checked)
                return PlayerColor.Green;
            if (red.Checked)
                return PlayerColor.Red;
            if (yellow.Checked)
                return PlayerColor.Yellow;
            if (!none.Checked)
                throw new InvalidOperationException("At least one of these radio buttons should have been checked. The gui is likely to blame");

            return 0;
        }

        private readonly GameBoard _game;
    }
}
