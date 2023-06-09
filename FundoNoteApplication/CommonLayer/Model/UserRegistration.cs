using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLayer.Model
{
    public class UserRegistration
    {
        private const string exp = "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";

        [Required(ErrorMessage ="FirstName is Mandatory")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="LastName is Mandatory")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Email is Mandatory")]
        [RegularExpression(exp)]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password Required")]
        public string Password { get; set; }

    }
}