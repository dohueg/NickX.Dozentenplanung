using System;

namespace NickX.Dozentenplanung.Utils
{
    public class XCalendarItem
    {
        public String Title { get; set; }
        public DateTime TimeStamp { get; set; }
        public int DurationInMinutes { get; set; }
        public XCalendarUser User { get; set; }
    }
}
