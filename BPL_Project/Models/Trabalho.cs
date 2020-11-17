using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BPL_Project.Models
{
    public class Trabalho
    {
        [Key]
        public virtual int Trabalho_Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DataType(DataType.MultilineText)]
        public virtual string Assunto { get; set; }

        [Display(Name = "Problema")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DataType(DataType.MultilineText)]
        public virtual string Problema { get; set; }

        [Display(Name = "Data Início")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public virtual DateTime Data_Inicio { get; set; }

        [Display(Name = "Data de Execução")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public virtual DateTime Data_Tranca { get; set; }

        [Display(Name = "Data Fim")]
        [Required(ErrorMessage = "Item obrigatório")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public virtual DateTime Data_Fim { get; set; }

        [Display(Name = "Status")]
        public virtual string Status { get; set; }

        [Display(Name = "Professor Autor")]
        public virtual int Prof_Id { get; set; }

        public virtual string Prof_Nome { get; set; }

        public virtual string Prof_Curso { get; set; }


        public virtual Professor Professor { get; set; }
        public virtual List<Atividade> Atividade { set; get; }
        public virtual List<Objetivo> Objetivo { set; get; }
        public virtual List<Produto> Produto { set; get; }
        public virtual List<Avaliacao> Avaliacao { get; set; }
        public virtual List<Prof_Convidado> Prof_Convidado { get; set; }

    }
}