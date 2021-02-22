using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Linq;

namespace NickX.Dozentenplanung.Utils
{
    public delegate void XCalendarCellClick(XCalendarCell ClickedCell);

    public partial class XCalendar : UserControl, INotifyPropertyChanged
    {
        public List<XCalendarUser> Users { get; set; }
        public DateTime StartDateTime { get; set; }
        [DefaultValue(DayOfWeek.Monday)]
        public DayOfWeek FirstDayOfWeek { get; set; }
        public XCalendarCell SelectedXCalendarCell { get; private set; }

        // Events
        public event XCalendarCellClick XCalendarCellClicked;

        public CalendarViews CalendarView
        {
            get
            {
                return _calendarView;
            }
            set
            {
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

        public void VisualizeCalendar()
        {
            grid.DataSource = null;
            grid.Columns.Clear();
            grid.Rows.Clear();

            var dt = new DataTable();

            var col_date = new DataColumn("DATUM", typeof(String));
            dt.Columns.Add(col_date);
            foreach (var user in Users)
            {
                var col_user = new DataColumn(user.Shortname.ToUpper(), typeof(String));
                dt.Columns.Add(col_user);
            }

            var row_count = (_calendarView == CalendarViews.Day) ? 1 : (_calendarView == CalendarViews.Week) ? 7 : DateTime.DaysInMonth(StartDateTime.Year, StartDateTime.Month);
            var date = (_calendarView == CalendarViews.Day) ? StartDateTime : (_calendarView == CalendarViews.Week) ? DateTimeExtensions.StartOfWeek(StartDateTime, FirstDayOfWeek) : new DateTime(StartDateTime.Year, StartDateTime.Month, 1);
            for (int i = 0; i < row_count; i++)
            {
                dt.Rows.Add(String.Format("{0}, {1}", new DateTimeFormatInfo().GetShortestDayName(date.DayOfWeek), date.ToString("dd.MM")));
                date = date.AddDays(1);
            }
            grid.DataSource = dt;

            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (col.Index != 0)
                {
                    var user_short_name = col.HeaderText;
                    var user = this.Users.Where(u => u.Shortname.ToUpper() == user_short_name.ToUpper()).First();
                    col.HeaderCell.Style.BackColor = user.Color;
                }
            }

            foreach (DataGridViewRow row in this.grid.Rows)
            {
                for (int i = 1; i < this.grid.ColumnCount; i++)
                {
                    var cal_cell = GetCalendarCellFromIndexes(row.Index, i);
                    var dgv_cell = row.Cells[i];
                    dgv_cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (cal_cell.Items != null)
                    {
                        if (cal_cell.Items.Where(item => item.LastsAllDay).Count() > 0)
                        {
                            dgv_cell.Value = cal_cell.Items.Where(item => item.LastsAllDay).First().Title;
                        }
                        else
                        {
                            var count = cal_cell.Items.Count();
                            dgv_cell.Value = (count > 0) ? count.ToString() : "";
                        }
                    }
                }
            }
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
            _calendarView = view;
            VisualizeCalendar();
            AdjustRowHeight();
        }

        private void XCalendar_Load(object sender, EventArgs e)
        {
            VisualizeCalendar();
            AdjustRowHeight();
        }

        int CalculateRowHeight()
        {
            var row_count = this.grid.Rows.Count;
            var full_height = this.grid.Height;
            var header_height = this.grid.ColumnHeadersHeight;
            var full_rows_height = full_height - header_height;
            var single_row_height = full_rows_height / row_count;
            return (int)single_row_height;
        }

        void AdjustRowHeight()
        {
            var new_row_height = CalculateRowHeight();
            foreach (DataGridViewRow row in grid.Rows)
            {
                row.Height = new_row_height;
            }
            //this.grid.RowTemplate.Height = new_row_height;
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

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.ColumnIndex == 0)
                e.Cell.Selected = false;
        }

        private void grid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
                SetCellBackColor(e.ColumnIndex, e.RowIndex, Color.White);
        }

        void SetCellBackColor(int x, int y, Color color)
        {
            var cell = grid.Rows[y].Cells[x];
            cell.Style.BackColor = color;
        }

        private void grid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
                SetCellBackColor(e.ColumnIndex, e.RowIndex, this.grid.BackgroundColor);
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                SelectedXCalendarCell = GetCalendarCellFromIndexes(e.RowIndex, e.ColumnIndex);
                XCalendarCellClicked?.Invoke(SelectedXCalendarCell);
            }
        }

        private XCalendarCell GetCalendarCellFromIndexes(int row_index, int col_index)
        {
            var retVal = new XCalendarCell();
            var grid_cell = this.grid.Rows[row_index].Cells[col_index];
            var str_user = this.grid.Columns[col_index].HeaderText;
            var user = this.Users.Where(u => u.Shortname.ToUpper() == str_user.ToUpper()).First();
            var dt = DateTime.Parse(this.grid.Rows[row_index].Cells[0].Value.ToString().Split(',')[1].Trim());

            retVal.RowIndex = row_index;
            retVal.ColumnIndex = col_index;
            retVal.User = user;
            retVal.DateTime = dt;
            return retVal;
        }

    }

    public enum CalendarViews
    {
        Day,
        Week,
        Month
    }
}
