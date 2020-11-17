using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPL_Project.Models;

namespace BPL_Project.Controllers
{
    public class ExperienciasController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

        

        // GET: Experiencias/Create
        public ActionResult Create()
        {
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            return View();
        }

        // POST: Experiencias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Experiencia_Id,Prof_Id,Prof_Nome,Descricao,Trabalho_Id")] Avalia_Geral experiencia)
        {
            if (ModelState.IsValid)
            {
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                experiencia.Prof_Id = prof.Prof_Id;
                experiencia.Trabalho_Id = trabalho_temp.Trabalho_Id;
                experiencia.Prof_Nome = prof.Nome;
                try
                {
                    db.Experiencias.Add(experiencia);
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewExperiencias");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(experiencia);
                }
                
            }

            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(experiencia);
        }

        // GET: Experiencias/Edit/5
        public ActionResult Edit(int? id)
        {
            Trabalho trabalhoTemp = TempData["Trabalho"] as Trabalho;//busca trabalho aberto
            TempData["Trabalho"] = trabalhoTemp;//guarda trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido!";
            }
            Avalia_Geral experiencia = db.Experiencias.Find(id);
            if (experiencia == null)
            {
                ViewData["Message"] = "Item não encontrado!";
            }
            return View(experiencia);
        }

        // POST: Experiencias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Experiencia_Id,Prof_Id,Prof_Nome,Descricao,Trabalho_Id")] Avalia_Geral experiencia)
        {
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                experiencia.Prof_Id = prof.Prof_Id;
                experiencia.Trabalho_Id = trabalho_temp.Trabalho_Id;
                experiencia.Prof_Nome = prof.Nome;
                try
                {
                    db.Entry(experiencia).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(experiencia);
                }
                
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(experiencia);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
