using System;
using System.Data.Entity;

namespace NickX.Dozentenplanung.Utils
{
    public class XDbContext : DbContext
    {
        public XDbContext(String connectionString) : base(connectionString)
        { }
        public XDbContext()
        { }

        public virtual DbSet<XCalendarUser> CalendarUsers { get; set; }
        public virtual DbSet<XCalendarItem> CalendarItems { get; set; }
        public virtual DbSet<XCalendarItemHistoryEntry> CalendarItemHistoryEntries { get; set; }
        public virtual DbSet<XSettingsEntry> SettingsEntries { get; set; }
    }
}
