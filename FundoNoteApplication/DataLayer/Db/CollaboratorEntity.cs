using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Db
{
    public  class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabId { get; set; }    
        public string CollaboratedEmail { get; set; }

        [ForeignKey("note")]
        public int NoteID { get; set; }
        public virtual NotesEntity note { get; set; }
        [ForeignKey("user")]
        public long UserId { get; set; }
        public virtual UserEntity user { get; set; }
    }
}
