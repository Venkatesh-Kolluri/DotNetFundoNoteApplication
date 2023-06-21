using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Db
{
    public class LabelEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }
        public string LabelName { get; set; }

        [ForeignKey("notes")]
        public int NoteId { get; set; }
        public virtual NotesEntity notes { get; set; }

        [ForeignKey ("users")]
        public long UserId { get; set; }
        public virtual UserEntity users { get; set; }
    }
}
