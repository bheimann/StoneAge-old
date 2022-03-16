using System;
using System.Windows.Forms;
using StoneAge.Core;
using StoneAge.Core.Models;

namespace StoneAge.WinForms.Controls
{
    public class LimitedLocation : Button
    {
        public BoardSpace Space { get; set; }

        protected Game Game => MainForm.Game;

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            //Game.TryToOccupySpace(Space);

            //BackColor = Game.ColorOfSpace(Space).ToDrawingColor(SystemColors.Control);
        }
    }
}
