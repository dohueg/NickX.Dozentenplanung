using System;
using System.Collections.Generic;
using System.Drawing;

namespace NickX.Dozentenplanung.Utils
{
    public class XCalendarUser
    {
        public String Name { get; set; }
        public String ShortName { get; set; }
        public Color Color { get; set; }
        public List<XCalendarItem> CalendarItems { get; set; }
    }
}
