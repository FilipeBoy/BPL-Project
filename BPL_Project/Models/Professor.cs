using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class Professor
    {
        [Key]
        public int Prof_Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Item obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Item obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        public string Curso_Nome { get; set; }

        [Display(Name = "Curso")]
        [Required(ErrorMessage = "Você precisa selecinar um curso")]
        public int Curso_Id { get; set; }

        public virtual Curso Curso { get; set; }
        public virtual ICollection<Trabalho> Trabalho { get; set; }
        public virtual ICollection<Atividade> Atividade { get; set; }
        public virtual ICollection<Produto> Produto { get; set; }
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }
        public virtual ICollection<Objetivo> Objetivo { get; set; }
        public virtual ICollection<Prof_Convidado> Prof_Convidado { get; set; }

    }
}