using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Item obrigatório")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Item obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Paper { get; set; }
        
    }
}