using System.Drawing;
using System.Windows.Forms;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Players;

namespace StoneAge.WinForms.Controls
{
    public partial class PlayerDisplay : UserControl
    {
        public PlayerDisplay()
        {
            InitializeComponent();
        }

        public void SetPlayerBoard(Player player)
        {
            _player = player;
            _playerBoard = player.PlayerBoard;

            RefreshControls();
        }

        public void RefreshControls()
        {
            labelPlayerName.Text = _player.Name;
            labelPlayerName.ForeColor = _player.Color.ToDrawingColor();
            labelScoreValue.Text = _playerBoard.Score.ToString();
            labelPeopleRemaining.Text = _playerBoard.PeopleToPlace.ToString();
            labelFoodTrackValue.Text = _playerBoard.FoodTrack.ToString();
            labelFoodTotal.Text = _playerBoard.Food.ToString();
            labelWoodRemaining.Text = _playerBoard.Resources[Resource.Wood].ToString();
            labelBrickRemaining.Text = _playerBoard.Resources[Resource.Brick].ToString();
            labelStoneRemaining.Text = _playerBoard.Resources[Resource.Stone].ToString();
            labelGoldRemaining.Text = _playerBoard.Resources[Resource.Gold].ToString();
            labelTool1.Text = "+" + _playerBoard.Tools[0].Value;
            labelTool1.Font = _playerBoard.Tools[0].Used ? new Font(labelTool1.Font, FontStyle.Strikeout) : new Font(labelTool1.Font, FontStyle.Regular);
            labelTool2.Text = "+" + _playerBoard.Tools[1].Value;
            labelTool2.Font = _playerBoard.Tools[1].Used ? new Font(labelTool2.Font, FontStyle.Strikeout) : new Font(labelTool2.Font, FontStyle.Regular);
            labelTool3.Text = "+" + _playerBoard.Tools[2].Value;
            labelTool3.Font = _playerBoard.Tools[2].Used ? new Font(labelTool3.Font, FontStyle.Strikeout) : new Font(labelTool3.Font, FontStyle.Regular);
        }

        private Player _player;
        private PlayerBoard _playerBoard;
    }
}
