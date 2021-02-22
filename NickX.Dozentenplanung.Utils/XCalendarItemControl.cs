using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NickX.Dozentenplanung.Utils
{
    public partial class XCalendarItemControl : UserControl
    {
        private XCalendarItem _item;
        private TransparentPanel _overlay;
        private ICollection<Panel> _border;

        public XCalendarItemControl(XCalendarItem calendar_item)
        {
            InitializeComponent();
            this._item = calendar_item;
            this.Dock = DockStyle.Top;

            _border = new List<Panel>()
            {
                border_left, border_top, border_right, border_bottom
            };
        }

        private void Overlay_MouseLeave(object sender, System.EventArgs e)
        {
            SetBorderColor(_item.User.Color);
        }

        private void Overlay_MouseEnter(object sender, System.EventArgs e)
        {
            SetBorderColor(Color.Black);
        }

        void SetBorderColor(Color color)
        {
            foreach (var b in _border)
            {
                b.BackColor = color;
            }
        }

        private void XCalendarItemControl_Load(object sender, System.EventArgs e)
        {
            _overlay = new TransparentPanel()
            {
                Height = this.Height - 2,
                Width = this.Width - 2,
                Location = new Point(1, 1)
            };
            this.Controls.Add(_overlay);
            _overlay.BringToFront();
            _overlay.MouseEnter += Overlay_MouseEnter;
            _overlay.MouseLeave += Overlay_MouseLeave;
            _overlay.Click += Overlay_Click;

            this.label_title.BackColor = _item.User.Color;
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.label_title.Text = _item.Title;
            this.label_start_time.Text = _item.DateBegin.ToString("hh.mm") + " Uhr";
            var dur_text = (_item.LastsAllDay) ? "Ganztägig" : _item.DurationInMinutes + " Minuten";
            this.label_duration.Text = dur_text;
            this.label_comment.Text = _item.Comment;
            SetBorderColor(_item.User.Color);
        }

        private void Overlay_Click(object sender, System.EventArgs e)
        {
            //MessageBox.Show("Clicked Item: " + _item.Id.ToString());
            label_start_time.Text = "blah";
        }
    }
}
