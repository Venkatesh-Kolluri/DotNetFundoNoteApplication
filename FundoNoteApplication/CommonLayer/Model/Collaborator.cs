using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
    public class Collaborator
    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        [Required(ErrorMessage = "Email is Mandatory")]
        public string CollaboratedEmail { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public int NoteId { get; set; }
        [RegularExpression(@"^[0-9]{1,5}$")]
        public long UserId { get; set; }
    }
}
