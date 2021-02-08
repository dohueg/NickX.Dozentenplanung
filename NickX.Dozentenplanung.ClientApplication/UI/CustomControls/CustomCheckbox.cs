using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NickX.Dozentenplanung.ClientApplication.UI.CustomControls
{
    public partial class CustomCheckbox : UserControl
    {
        public String LabelText { get { return text_label.Text; } set { text_label.Text = value; } }
        [DefaultValue(false)]
        public bool Checked { get; set; }
        public Color ColorChecked { get; set; }
        public Color ColorUnchecked { get; set; }
        public Color ColorHover { get; set; }

        public CustomCheckbox()
        {
            InitializeComponent();
            this.ColorChecked = Color.FromArgb(42, 191, 159);
            this.ColorUnchecked = Color.Gainsboro;
            this.ColorHover = Color.Silver;
        }

        private void CustomCheckbox_Load(object sender, EventArgs e)
        {
            UpdateCheckedColor();
        }

        void UpdateCheckedColor()
        {
            check_panel.BackColor = Checked ? this.ColorChecked : this.ColorUnchecked;
        }

        private void check_panel_MouseEnter(object sender, EventArgs e)
        {
            if (!Checked)
            {
                check_panel.BackColor = this.ColorHover;
            }
        }

        private void check_panel_MouseLeave(object sender, EventArgs e)
        {
            if (!Checked)
            {
                check_panel.BackColor = this.ColorUnchecked;
            }
        }

        private void check_panel_Click(object sender, EventArgs e)
        {
            this.Checked = !this.Checked;
            UpdateCheckedColor();
        }
    }
}
