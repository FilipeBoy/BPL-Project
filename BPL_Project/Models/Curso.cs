using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class Curso
    {
        [Key]
        public int Curso_Id { get; set; }

        [Display(Name = "Curso")]
        [Required(ErrorMessage = "Item obrigatório")]
        public string Nome { get; set; }

        public virtual ICollection<Professor> Professor { get; set; }
    }
}