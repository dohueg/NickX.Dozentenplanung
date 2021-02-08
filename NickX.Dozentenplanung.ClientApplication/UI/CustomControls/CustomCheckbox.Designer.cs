
namespace NickX.Dozentenplanung.ClientApplication.UI.CustomControls
{
    partial class CustomCheckbox
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.check_panel = new System.Windows.Forms.Panel();
            this.text_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // check_panel
            // 
            this.check_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.check_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.check_panel.Location = new System.Drawing.Point(0, 10);
            this.check_panel.Name = "check_panel";
            this.check_panel.Size = new System.Drawing.Size(15, 15);
            this.check_panel.TabIndex = 0;
            this.check_panel.Click += new System.EventHandler(this.check_panel_Click);
            this.check_panel.MouseEnter += new System.EventHandler(this.check_panel_MouseEnter);
            this.check_panel.MouseLeave += new System.EventHandler(this.check_panel_MouseLeave);
            // 
            // text_label
            // 
            this.text_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.text_label.Location = new System.Drawing.Point(15, 10);
            this.text_label.Name = "text_label";
            this.text_label.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.text_label.Size = new System.Drawing.Size(267, 15);
            this.text_label.TabIndex = 1;
            this.text_label.Text = "label1";
            this.text_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CustomCheckbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.text_label);
            this.Controls.Add(this.check_panel);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "CustomCheckbox";
            this.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.Size = new System.Drawing.Size(292, 35);
            this.Load += new System.EventHandler(this.CustomCheckbox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel check_panel;
        private System.Windows.Forms.Label text_label;
    }
}
