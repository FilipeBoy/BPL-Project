using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BPL_Project.Models;

namespace BPL_Project.Controllers
{
    public class HomeController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

        public ActionResult Index()
        {
             return View();
        }

        public ActionResult Inicio()
        {
            TempData["Page"] = null;
            if (Session["prof"] != null)//verifica se algum professor está logado
            {
                var prof = Session["prof"] as Professor;//busca informações do professor logado
                var Trab_convidado = db.Prof_Convidado.Where(x => x.Prof_Id.Equals(prof.Prof_Id)).FirstOrDefault();
                var trabalhos = db.Trabalhoes.Where(x => x.Prof_Id.Equals(prof.Prof_Id)).FirstOrDefault();
                if (Trab_convidado == null && trabalhos == null)// verifica se o professor  possui algum trabalho ou convidado para algum trabalho
                {

                    return View();
                }

                return RedirectToAction("../Trabalhoes/Index");
            }
            return View();
        }
     }
}