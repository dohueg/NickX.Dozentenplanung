using NickX.Dozentenplanung.Utils;
using System;

namespace NickX.Dozentenplanung.ClientApplication
{
    public class Session
    {
        public String ConnectionString { get; set; }
        public XCalendarUser CurrentUser { get; set; }
    }
}
