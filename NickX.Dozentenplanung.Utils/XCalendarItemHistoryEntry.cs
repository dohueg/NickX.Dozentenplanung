using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NickX.Dozentenplanung.Utils
{
    [Table("tbl_calendar_item_history")]
    public class XCalendarItemHistoryEntry
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("timestamp")]
        public String Timestamp { get; set; }
        [Column("username")]
        public String Username { get; set; }
        [Column("change")]
        public String Change { get; set; }
        [Column("ref_calendar_item")]
        [ForeignKey("CalendarItem")]
        public int CalendarItemId { get; set; }

        public virtual XCalendarItem CalendarItem { get; set; }
    }
}
