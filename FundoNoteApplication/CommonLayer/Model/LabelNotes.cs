using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class LabelNotes
    {
        public string LabelName { get; set; }
        public int NoteId { get; set; }
        public long UserId { get; set; }
    }
}
