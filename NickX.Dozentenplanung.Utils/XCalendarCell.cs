using System;
using System.Linq;
using System.Collections.Generic;

namespace NickX.Dozentenplanung.Utils
{
    public class XCalendarCell
    {
        public XCalendarUser User { get; set; }
        public DateTime DateTime { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public List<XCalendarItem> Items { get { return GetItems(); } }

        private List<XCalendarItem> GetItems()
        {
            if (User.CalendarItems == null)
                return null;
            return User.CalendarItems.Where(i => i.DateBegin.Date == DateTime.Date).ToList();
        }
    }
}
