using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NickX.Dozentenplanung.Utils
{
    [Table("tbl_calendar_items")]
    public class XCalendarItem
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public String Title { get; set; }
        [Column("date_begin")]
        public DateTime DateBegin { get; set; }
        [Column("duration")]
        public int DurationInMinutes { get; set; }
        [Column("lasts_all_day")]
        public bool LastsAllDay { get; set; }
        [Column("ref_user")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Column("comment")]
        public String Comment { get; set; }

        public virtual XCalendarUser User { get; set; }
        public virtual ICollection<XCalendarItemHistoryEntry> HistoryEntries { get; set; }
    }
}
