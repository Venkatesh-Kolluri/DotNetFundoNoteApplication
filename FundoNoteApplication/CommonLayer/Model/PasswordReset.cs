using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class PasswordReset
    {
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
    }
}
