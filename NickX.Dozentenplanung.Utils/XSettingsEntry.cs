using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NickX.Dozentenplanung.Utils
{
    [Table("tbl_settings")]
    public class XSettingsEntry
    {
        [Key]
        [Column("prop_key")]
        public String Key { get; set; }
        [Column("prop_value")]
        public String Value { get; set; }
        [Column("prop_section")]
        public String Section { get; set; }
    }
}
