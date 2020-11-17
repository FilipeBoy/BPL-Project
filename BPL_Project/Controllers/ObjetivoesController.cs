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
    public class ObjetivoesController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();


        // GET: Objetivoes/Create
        public ActionResult Create()
        {
            TempData["item"] = "objetivo";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            return View();
        }

        // POST: Objetivoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Objetivo_Id,Descricao,Trabalho_Id")] Objetivo objetivo)
        {
            TempData["item"] = "objetivo";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                objetivo.Prof_Id = prof.Prof_Id;
                objetivo.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Objetivoes.Add(objetivo);
                    db.SaveChanges();
                    TempData["Message"] = "Cadastrado com sucesso";
                    return RedirectToAction("Create");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(objetivo);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(objetivo);
        }

        // GET: Objetivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            TempData["item"] = "objetivo";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Objetivo objetivo = db.Objetivoes.Find(id);
            if (objetivo == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(objetivo);
        }

        // POST: Objetivoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Objetivo_Id,Descricao,Trabalho_Id")] Objetivo objetivo)
        {
            TempData["item"] = "objetivo";
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                objetivo.Prof_Id = prof.Prof_Id;
                objetivo.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Entry(objetivo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(objetivo);
                }
                
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(objetivo);
        }

        // GET: Objetivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            TempData["item"] = "objetivo";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Objetivo objetivo = db.Objetivoes.Find(id);
            if (objetivo == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(objetivo);
        }

        // POST: Objetivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["item"] = "objetivo";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            Objetivo objetivo = db.Objetivoes.Find(id);
            try
            {
                db.Objetivoes.Remove(objetivo);
                db.SaveChanges();
                return RedirectToAction("../Trabalhoes/ViewTrabalho1");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, tente novamente mais tarde";
                return View(objetivo);
            }
            
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
