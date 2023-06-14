using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Db
{
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteID { get; set; } 
        public string Title { get; set; }
        public string Note { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }
        public DateTime? Remind_Me { get; set; }
        public DateTime? Createat { get; set; }

        [ForeignKey("Users")]
        public long UserId { get; set; }
        public virtual UserEntity user { get; set; }
    }
}
