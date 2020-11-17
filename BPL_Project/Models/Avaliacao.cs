using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class Avaliacao
    {
        [Key]
        public int Avalia_Trabalho_Id { get; set; }

        public int Prof_Id { get; set; }

        [Display(Name = "Trabalho")]
        public int Trabalho_Id { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        public virtual Professor Professor { get; set; }

    }
}