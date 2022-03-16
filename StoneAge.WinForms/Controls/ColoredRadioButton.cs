using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace StoneAge.WinForms.Controls
{
    public class ColoredRadioButton : RadioButton
    {
        private Color _onColour;
        private Color _offColour;
        private readonly Rectangle _glint;
        private readonly Rectangle _circle;
        private readonly PathGradientBrush _flareBrush;
        private readonly Pen _outline;

        public Color OnColour
        {
            get
            {
                return _onColour;
            }
            set
            {
                if (value == Color.White || value == Color.Transparent)
                    _onColour = Color.Empty;
                else
                    _onColour = value;
            }
        }
        public Color OffColour
        {
            get
            {
                return _offColour;
            }
            set
            {
                if (value == Color.White || value == Color.Transparent)
                    _offColour = Color.Empty;
                else
                    _offColour = value;
            }
        }

        public ColoredRadioButton()
        {
            _circle = new Rectangle(2, 5, 7, 7);
            _glint = new Rectangle(3, 6, 4, 4);
            _outline = new Pen(new SolidBrush(Color.Black), 1F);

            GraphicsPath Path = new GraphicsPath();
            Path.AddEllipse(_glint);
            _flareBrush = new PathGradientBrush(Path);
            _flareBrush.CenterColor = Color.White;
            _flareBrush.SurroundColors = new[] { Color.Transparent };
            _flareBrush.FocusScales = new PointF(0.5F, 0.5F);

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (Checked)
            {
                if (OnColour != Color.Empty)
                {
                    g.FillEllipse(new SolidBrush(OnColour), _circle);
                    g.FillEllipse(_flareBrush, _glint);
                    g.DrawEllipse(_outline, _circle);
                }
            }
            else
            {
                if (OffColour != Color.Empty)
                {
                    g.FillEllipse(new SolidBrush(OffColour), _circle);
                    g.FillEllipse(_flareBrush, _glint);
                    g.DrawEllipse(_outline, _circle);
                }
            }
        }
    }
}
