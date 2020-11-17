using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BPL_Project.Models;

namespace BPL_Project.Controllers
{
    public class RelatorioController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();
        // GET: Ralatorio
        public ActionResult Index()
        {
            return View();
        }

        //lista todos os trabalhos
        public List<Trabalho> ListTrabalho(out int totalRecord)
        {
            var data = db.Trabalhoes.ToList();
            totalRecord = data.Count();
            return data;
        }

        //busca trabalhos por palavra chave
        public List<Trabalho> GetTrabalho(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (BPL_ProjectContext db = new BPL_ProjectContext())
            {
                var v = (from a in db.Trabalhoes.Include(x=> x.Professor)
                         where
                                 a.Prof_Curso.ToUpper().Contains(search.ToUpper()) ||
                                 a.Prof_Nome.ToUpper().Contains(search.ToUpper()) ||
                                 a.Assunto.ToUpper().Contains(search.ToUpper()) ||
                                 a.Problema.ToUpper().Contains(search.ToUpper()) ||
                                 a.Professor.Nome.ToUpper().Contains(search.ToUpper()) ||
                                 a.Status.ToUpper().Contains(search.ToUpper())
                                 
                         select a
                                );
                
                totalRecord = v.Count();
                return v.ToList();
            }
        }
        //aplica busca em cima de busca
        public List<Trabalho> GetSecunTrabalho(List<Trabalho> Lista, string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var v = new List<Trabalho>();
            foreach (var a in Lista)
            {
                if(a.Prof_Curso.ToUpper().Contains(search.ToUpper()) ||
                                 a.Prof_Nome.ToUpper().Contains(search.ToUpper()) ||
                                 a.Assunto.ToUpper().Contains(search.ToUpper()) ||
                                 a.Problema.ToUpper().Contains(search.ToUpper()) ||
                                 a.Professor.Nome.ToUpper().Contains(search.ToUpper()) ||
                                 a.Status.ToUpper().Contains(search.ToUpper()))
                {
                    v.Add(a);
                }
            }
            
            totalRecord = v.Count();
            return v.ToList();
            
        }

        //retorna pagina de relatorios de trabalhos
        public ActionResult RelatorioTrabalho(int page = 1, string sort = "Nome", string sortdir = "asc", string search = "")
        {
            int pageSize = 2;//db.Trabalhoes.Count();//itens por página
            if (TempData["TotalRecord"] != null)
            {
                pageSize = int.Parse(TempData["TotalRecord"].ToString());//itens por página
            }
            
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            List<Trabalho> data = new List<Trabalho>();//cria uma nova lista
            if (search.Equals(""))//se não tiver palavra de busca
            {
                data = ListTrabalho(out totalRecord);//lista todos os trabalhos
            }else//se tiver palavra chave
            {
                string[] itemSearch = search.Split(' ');//divide a palavra chave em sub palavras
                data = GetTrabalho(itemSearch[0], sort, sortdir, skip, pageSize, out totalRecord);//realiza a primeira busca
                if (itemSearch.Count() > 1)//se mais de uma palavra
                {
                    for (int i = 1; i < itemSearch.Count(); i++)//realiza as demais buscas a partir das buscas anteriores
                    {
                        data = GetSecunTrabalho(data, itemSearch[i], sort, sortdir, skip, pageSize, out totalRecord);
                    }

                }
            }
            
            
            ViewBag.TotalRows = totalRecord;//total de itens encontrados
            ViewBag.search = search;
            return View(data);
        }
    }
}