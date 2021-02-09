using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NickX.Dozentenplanung.Utils
{
    public partial class XCalendar : UserControl, INotifyPropertyChanged
    {
        public List<XCalendarUser> Users { get; set; }
        public DateTime StartDateTime { get; set; }
        [DefaultValue(DayOfWeek.Monday)]
        public DayOfWeek FirstDayOfWeek { get; set; }
        public Color FillColorCurrentDateRow { get; set; }
        public Color BorderColorGrid { get; set; }
        public Color BorderColorGridRow { get; set; }
        public Color BorderColorGridColumn { get; set; }
        public Color FillColorWeekendDays { get; set; }
        private List<Panel> ItemPanels;

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
            this.ItemPanels = new List<Panel>();
        }

        public void PopulateGrid()
        {
            //this.Controls.Clear();
            //var col_count = this.Users.Count;
            //var row_count = 0;
            //switch (_calendarView)
            //{
            //    case CalendarViews.Day:
            //        row_count = 1;
            //        break;
            //    case CalendarViews.Week:
            //        row_count = 7;
            //        break;
            //    case CalendarViews.Month:
            //        row_count = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //        break;
            //}
            //var full_width = this.Width - 10;
            //var full_height = this.Height - 10;

            //var desc_col_height = 30;
            //var desc_row_width = 65;

            //var all_cols_width = full_width - desc_row_width;
            //var all_rows_height = full_height - desc_col_height;

            //var single_col_width = all_cols_width / col_count;
            //var single_row_height = all_rows_height / row_count;

            //var x = 10;
            //var y = 10;
            //for (int row_index = 0; row_index < row_count; row_count++)
            //{
            //    for (int col_index = 0; col_index < col_count; col_count++)
            //    {
            //        var item_panel = new Panel()
            //        {
            //            Width = single_col_width,
            //            Height = single_row_height,
            //            Location = new Point(x, y)
            //        };
            //        item_panel.MouseEnter += ItemPanel_MouseEnter;
            //        item_panel.MouseLeave += ItemPanel_MouseLeave;

            //        x += single_col_width;
            //        Console.WriteLine("Added Panel -> " + item_panel.Location.ToString());
            //        this.Controls.Add(item_panel);
            //    }
            //    y += single_row_height;
            //}

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
            PopulateGrid();
        }

        private void XCalendar_Load(object sender, EventArgs e)
        {
            PopulateGrid();
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
            var description_column_width = 65;

            // Calculate Full 
            int full_width = this.Width - 1 - description_column_width;
            int full_height = this.Height - 1 - description_row_height;

            if (this.Users.Count == 0)
            {
                var s = "Keine Benutzer gefunden.";
                var f = new Font(new FontFamily("Trebuchet MS"), 12f);
                var s_size = e.Graphics.MeasureString(s, f);
                var p = new Point((int)(full_width / 2) - (int)(s_size.Width / 2) + 30, 30);
                e.Graphics.DrawString(s, f, new SolidBrush(Color.FromArgb(180, 180, 180)), p);
                return;
            }

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

            // Draw Description Column BackColor
            var desc_col_rect = new Rectangle(new Point(1, description_row_height + 1), new Size(description_column_width - 1, full_height - 1));
            e.Graphics.FillRectangle(new SolidBrush(bez_pen.Color), desc_col_rect);

            // Draw Border
            var rect = new Rectangle(new Point(0, 0), new Size(this.Width - 1, this.Height - 1));
            e.Graphics.DrawRectangle(pen_border, rect);

            var desc_font = new Font(new FontFamily("Trebuchet MS"), 9f);
            var desc_brush = new SolidBrush(Color.Black);

            // Draw Columns
            var col_x = description_column_width;
            for (int x = 0; x < col_count; x++)
            {
                var rect_col = new Rectangle(new Point(col_x, 1), new Size(col_width, description_row_height));
                var user = Users[x];
                var short_name = user.Shortname;
                var desc_size = e.Graphics.MeasureString(short_name, desc_font);
                e.Graphics.FillRectangle(new SolidBrush(user.Color), rect_col);
                e.Graphics.DrawLine(pen_column_border, new Point(col_x, 0), new Point(col_x, full_height + description_row_height));
                e.Graphics.DrawString(short_name, desc_font, desc_brush, new Point(col_x + (int)(col_width / 2 - desc_size.Width / 2), (int)(description_row_height / 2 - desc_size.Height / 2)));

                col_x += col_width;
            }

            // Draw Rows
            var row_y = description_row_height;
            DateTime dt = this.StartDateTime == null ? DateTime.Now : StartDateTime;
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
                var rect_row = new Rectangle(new Point(1, row_y), new Size(description_column_width - 1, row_height));
                var s = new DateTimeFormatInfo().GetShortestDayName(dt.DayOfWeek) + ", " + dt.ToString("dd.MM");
                var cur_row_fill_color = Color.Transparent;
                if (dt.Date == DateTime.Now.Date)
                {
                    cur_row_fill_color = this.FillColorCurrentDateRow;
                }
                else if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    cur_row_fill_color = this.FillColorWeekendDays;
                }
                var s_size = e.Graphics.MeasureString(s, desc_font);
                e.Graphics.FillRectangle(new SolidBrush(cur_row_fill_color), rect_row);
                e.Graphics.DrawLine(pen_row_border, new Point(0, row_y), new Point(full_width + description_column_width, row_y));
                e.Graphics.DrawString(s, desc_font, desc_brush, new Point((int)(description_column_width / 2 - s_size.Width / 2), row_y + 3));

                dt = dt.AddDays(1);
                row_y += row_height;
            }
        }

        private void ItemPanel_MouseEnter(object sender, EventArgs e)
        {
            var p = (Panel)sender;
            p.BackColor = Color.FromArgb(255, 194, 0);
        }
        private void ItemPanel_MouseLeave(object sender, EventArgs e)
        {
            var p = (Panel)sender;
            p.BackColor = Color.Transparent;
        }
    }

    public enum CalendarViews
    {
        Day,
        Week,
        Month
    }
}
