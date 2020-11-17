using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class Prof_Convidado
    {
        [Key]
        public int Prof_Conv_Id { get; set; }

        [Display(Name = "Professor")]
        [Required(ErrorMessage = "Item obrigatório")]
        public int Prof_Id { get; set; }

        [Display(Name = "Permissao")]
        [Required(ErrorMessage = "Item obrigatório")]
        public string Permissao { get; set; }

        [Display(Name = "Trabalho")]
        public int Trabalho_Id { get; set; }
        

        public virtual Professor Professor { get; set; }
        public virtual Trabalho Trabalho { get; set; }

    }
}