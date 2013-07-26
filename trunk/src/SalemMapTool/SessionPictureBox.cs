using System;
using System.Drawing;
using System.Windows.Forms;

namespace SalemMapTool
{
    public partial class SessionPictureBox : UserControl
    {
        #region Data Members

        private bool _ctrlPressed;
        private bool _shiftPressed;

        private int _x0, _y0, _x, _y;

        private Session _session;

        #endregion

        #region Properties

        private bool _updating;
        public bool IsUpdating
        {
            get { return IsUpdating; }
        }

        #endregion

        public SessionPictureBox()
        {
            InitializeComponent();

            hScrollBar.Enabled = false;
            vScrollBar.Enabled = false;
            trackBarZoom.Visible = false;

            pictureBox.BackColor = (Color)Common.Instance.Parameters[Consts.s_backColor];
            pictureBox.MouseEnter += pictureBox_MouseEnter;
            pictureBox.MouseWheel += pictureBox_MouseWheel;
            pictureBox.KeyDown += pictureBox_PreviewKey;
            pictureBox.KeyUp += pictureBox_PreviewKey;
            pictureBox.LostFocus += pictureBox_LostFocus;
        }

        public void UpdateSession(Session newSession)
        {
            _session = newSession;
            hScrollBar.Enabled = _session != null;
            vScrollBar.Enabled = _session != null;
            trackBarZoom.Visible = _session != null;

            UpdateBars();

            pictureBox.Refresh();
        }

        void UpdateBars()
        {
            if (_session != null)
            {
                _updating = true;

                _session.SetFOV(new Rectangle(10, 10, pictureBox.Width - 20, pictureBox.Height - 20));

                hScrollBar.LargeChange = _session.FOVWidth;
                hScrollBar.SmallChange = hScrollBar.LargeChange / 4;
                hScrollBar.Maximum = _session.Width;
                hScrollBar.Value = _session.FOVLeft;

                vScrollBar.LargeChange = _session.FOVHeight;
                vScrollBar.SmallChange = vScrollBar.LargeChange / 4;
                vScrollBar.Maximum = _session.Height;
                vScrollBar.Value = _session.FOVTop;

                trackBarZoom.Minimum = _session.ZoomMin;
                trackBarZoom.Maximum = _session.ZoomMax;
                trackBarZoom.Value = _session.Zoom;

                _updating = false;
            }
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (_session == null || _updating)
                return;

            _session.FOVLeft = hScrollBar.Value;

            pictureBox.Refresh();
        }
        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (_session == null || _updating)
                return;

            _session.FOVTop = vScrollBar.Value;

            pictureBox.Refresh();
        }
        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            if (_session == null || _updating)
                return;

            _session.SetZoom(trackBarZoom.Value, pictureBox.Width / 2, pictureBox.Height / 2);

            UpdateBars();

            pictureBox.Refresh();
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            if (_session == null || pictureBox.Width == 0 || pictureBox.Height == 0)
                return;

            _session.SetFOV(new Rectangle(10, 10, pictureBox.Width - 20, pictureBox.Height - 20));

            UpdateBars();

            pictureBox.Refresh();
        }
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_session == null)
                e.Graphics.Clear(pictureBox.BackColor);
            else
                _session.Draw(e.Graphics);
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_session == null || e.Button != MouseButtons.Left)
                return;

            if (_shiftPressed)
            {
                _session.StartSelect(e.X, e.Y);

                pictureBox.Refresh();
            }

            _x0 = _x = e.X;
            _y0 = _y = e.Y;
        }
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (_session == null || e.Button != MouseButtons.Left)
                return;

            if (_shiftPressed)
            {
                _session.EndSelect(Math.Abs(_x0 - e.X) <= 1 && Math.Abs(_y0 - e.Y) <= 1);

                pictureBox.Refresh();
            }
            else if (_ctrlPressed && Math.Abs(_x0 - e.X) <= 1 && Math.Abs(_y0 - e.Y) <= 1)
            {
                _session.Choose(e.X, e.Y);

                pictureBox.Refresh();
            }
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_session == null || e.Button != MouseButtons.Left)
                return;


            if (_shiftPressed)
            {
                _session.Move(e.X, e.Y);
            }
            else
            {
                _session.Move(e.X - _x, e.Y - _y);

                UpdateBars();
            }

            pictureBox.Refresh();

            _x = e.X;
            _y = e.Y;
        }
        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            pictureBox.Focus();
        }
        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_session == null || _shiftPressed)
                return;

            _session.SetZoom(_session.Zoom + (e.Delta > 0 ? 1 : -1), e.X, e.Y);

            UpdateBars();

            pictureBox.Refresh();
        }
        private void pictureBox_PreviewKey(object sender, KeyEventArgs e)
        {
            _ctrlPressed = e.Control;
            _shiftPressed = e.Shift;
        }
        private void pictureBox_LostFocus(object sender, EventArgs e)
        {
            _ctrlPressed = false;
            _shiftPressed = false;
        }
    }
}
