using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Eyedia.Aarbac.Framework
{
    public class RbacRegisterUser
    {       
        public int RoleId { get; set; }
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public RbacRegisterUser() { }
        public RbacRegisterUser(int roleId, string userName, string fullName, string email, string password)
        {            
            this.RoleId = roleId;
            this.UserName = userName;
            this.FullName = fullName;
            this.Email = email;
            this.Password = password;
        }
    }

    public class SetPassword
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string NewPassword { get; set; }
    }
}
