using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class Avalia_Geral
    {
        [Key]
        public int Experiencia_Id { get; set; }

        public int Prof_Id { get; set; }

        public string Prof_Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Display(Name = "Trabalho")]
        public int Trabalho_Id { get; set; }

        public virtual Professor Professor { get; set; }
    }
}