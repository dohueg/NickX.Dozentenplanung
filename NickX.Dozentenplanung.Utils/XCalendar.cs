using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NickX.Dozentenplanung.Utils
{
    public partial class XCalendar : UserControl, INotifyPropertyChanged
    {
        public List<XCalendarUser> Users { get; set; }

        [DefaultValue(DayOfWeek.Monday)]
        public DayOfWeek FirstDayOfWeek { get; set; }

        public Color BorderColorGrid { get; set; }
        public Color BorderColorGridRow { get; set; }
        public Color BorderColorGridColumn { get; set; }

        public CalendarViews CalendarView
        {
            get
            {
                return _calendarView;
            }
            set
            {
                _calendarView = value;
                ApplyCalendarView(value);
                NotifyPropertyChanged();
            }
        }
        [DefaultValue(CalendarViews.Week)]
        private CalendarViews _calendarView;

        public XCalendar()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Users = new List<XCalendarUser>();
        }

        #region Property Changed Handling
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void ApplyCalendarView(CalendarViews view)
        {
            this.Invalidate();
        }

        private void XCalendar_Load(object sender, EventArgs e)
        {
        }

        private void XCalendar_Paint(object sender, PaintEventArgs e)
        {
            // Pen
            var pen_border = new Pen(BorderColorGrid);
            var pen_row_border = new Pen(BorderColorGridRow);
            var pen_column_border = new Pen(BorderColorGridColumn);

            //var grid_pen = new Pen(Color.Black);
            var bez_pen = new Pen(Color.FromArgb(192, 241, 231));

            var description_row_height = 30;
            var description_column_width = 60;

            // Calculate Full 
            int full_width = this.Width - 1 - description_column_width;
            int full_height = this.Height - 1 - description_row_height;

            // Calculate Rows
            int row_count = 0;
            switch (_calendarView)
            {
                case CalendarViews.Day:
                    row_count = 1;
                    break;
                case CalendarViews.Week:
                    row_count = 7;
                    break;
                case CalendarViews.Month:
                    row_count = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    break;
            }
            int row_height = full_height / row_count;

            // Calculate Columns
            int col_count = Users.Count;
            int col_width = full_width / col_count;

            // Draw Description Row & Column BackColor
            var desc_col_rect = new Rectangle(new Point(1, description_row_height + 1), new Size(description_column_width - 1, full_height - 1));
            var desc_row_rect = new Rectangle(new Point(description_column_width + 1, 1), new Size(full_width - 1, description_row_height - 1));
            e.Graphics.FillRectangle(new SolidBrush(bez_pen.Color), desc_col_rect);
            e.Graphics.FillRectangle(new SolidBrush(bez_pen.Color), desc_row_rect);

            // Draw BackColor
            //if (Users.Count > 0)
            //{
            //    var col_rect = new Rectangle(new Point(1, 1), new Size(col_width - 1, full_height - 1));
            //    var row_rect = new Rectangle(new Point(col_width + 1, 1), new Size(full_width - col_width - 1, row_height - 1));
            //    e.Graphics.FillRectangle(new SolidBrush(bez_pen.Color), col_rect);
            //    e.Graphics.FillRectangle(new SolidBrush(bez_pen.Color), row_rect);
            //}

            // Draw Border
            var rect = new Rectangle(new Point(0, 0), new Size(this.Width - 1, this.Height - 1));
            e.Graphics.DrawRectangle(pen_border, rect);

            var desc_font = new Font(new FontFamily("Trebuchet MS"), 9f);
            var desc_brush = new SolidBrush(Color.Black);

            // Draw Columns
            if (Users.Count > 0)
            {
                var col_x = description_column_width;
                for (int x = 0; x < col_count; x++)
                {
                    var user = Users[x];
                    var short_name = user.ShortName;
                    var desc_size = e.Graphics.MeasureString(short_name, desc_font);
                    e.Graphics.DrawLine(pen_column_border, new Point(col_x, 0), new Point(col_x, full_height + description_row_height));
                    e.Graphics.DrawString(user.ShortName, desc_font, desc_brush, new Point(col_x + (int)(col_width / 2 - desc_size.Width / 2), (int)(description_row_height / 2 - desc_size.Height / 2)));
                    col_x += col_width;
                }
            }

            // Draw Rows
            var row_y = description_row_height;
            DateTime dt = DateTime.Now.AddDays(10);
            switch (this._calendarView)
            {
                case CalendarViews.Week:
                    dt = DateTimeExtensions.StartOfWeek(dt, DayOfWeek.Monday);
                    break;
                case CalendarViews.Month:
                    dt = new DateTime(dt.Year, dt.Month, 1);
                    break;
            }
            for (int x = 0; x < row_count; x++)
            {
                var s = dt.ToString("dd.MM");
                var s_size = e.Graphics.MeasureString(s, desc_font);
                e.Graphics.DrawLine(pen_row_border, new Point(0, row_y), new Point(full_width + description_column_width, row_y));
                e.Graphics.DrawString(s, desc_font, desc_brush, new Point((int)(description_column_width / 2 - s_size.Width / 2), row_y + 3));

                dt = dt.AddDays(1);
                row_y += row_height;
            }
        }
    }

    public enum CalendarViews
    {
        Day,
        Week,
        Month
    }
}
