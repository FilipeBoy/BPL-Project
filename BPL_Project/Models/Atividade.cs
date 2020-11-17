using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class Atividade
    {
        [Key]
        public int Atividade_Id { get; set; }

        public int Prof_Id { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Display(Name = "Data Execução")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Data_Exec { get; set; }

        [Display(Name = "Trabalho")]
        public int Trabalho_Id { get; set; }

        public virtual Professor Professor { get; set; }
    }
}