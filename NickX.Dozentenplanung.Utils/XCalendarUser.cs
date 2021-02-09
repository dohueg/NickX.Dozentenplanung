using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace NickX.Dozentenplanung.Utils
{
    [Table("tbl_user")]
    public class XCalendarUser
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public String Name { get; set; }
        [Column("shortname")]
        public String Shortname { get; set; }
        [Column("color_r")]
        public int ColorR { get; set; }
        [Column("color_g")]
        public int ColorG { get; set; }
        [Column("color_b")]
        public int ColorB { get; set; }


        public virtual ICollection<XCalendarItem> CalendarItems { get; set; }

        [NotMapped]
        public Color Color
        {
            get
            {
                return Color.FromArgb(ColorR, ColorG, ColorB);
            }
            set
            {
                ColorR = value.R;
                ColorG = value.G;
                ColorB = value.B;
            }
        }
    }
}
