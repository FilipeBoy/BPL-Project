using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BPL_Project.Models
{
    public class BPL_ProjectContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BPL_ProjectContext() : base("name=BPL_ProjectContext"){}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<BPL_Project.Models.Atividade> Atividades { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Trabalho> Trabalhoes { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Avaliacao> Avalia_Trabalho { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Curso> Cursoes { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Objetivo> Objetivoes { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Produto> Produtoes { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Professor> Professors { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Prof_Convidado> Prof_Convidado { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<BPL_Project.Models.Avalia_Geral> Experiencias { get; set; }
    }
}
