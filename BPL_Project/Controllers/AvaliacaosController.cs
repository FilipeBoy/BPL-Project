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
    public class AvaliacaosController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

       

        // GET: Avaliacaos/Create
        public ActionResult Create()
        {
            TempData["item"] = "avaliacao";//Serve para voltar na mesma posição na pagina do trabalho
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            return View();
        }

        // POST: Avaliacaos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Avalia_Trabalho_Id,Trabalho_Id,Descricao")] Avaliacao avaliacao)
        {
            TempData["item"] = "avaliacao";//Serve para voltar na mesma posição na pagina do trabalho
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                avaliacao.Prof_Id = prof.Prof_Id;
                avaliacao.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Avalia_Trabalho.Add(avaliacao);
                    db.SaveChanges();
                    TempData["Message"] = "Cadastrado com sucesso!";
                    return RedirectToAction("Create");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(avaliacao);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(avaliacao);
        }

        // GET: Avaliacaos/Edit/5
        public ActionResult Edit(int? id)
        {
            TempData["item"] = "avaliacao";//Serve para voltar na mesma posição na pagina do trabalho
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido!";
            }
            Avaliacao avaliacao = db.Avalia_Trabalho.Find(id);
            if (avaliacao == null)
            {
                ViewData["Message"] = "Item não encontrado!";
            }
            return View(avaliacao);
        }

        // POST: Avaliacaos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Avalia_Trabalho_Id,Trabalho_Id,Descricao")] Avaliacao avaliacao)
        {
            TempData["item"] = "avaliacao";//Serve para voltar na mesma posição na pagina do trabalho
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                avaliacao.Prof_Id = prof.Prof_Id;
                avaliacao.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Entry(avaliacao).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(avaliacao);
                }
                
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(avaliacao);
        }

        // GET: Avaliacaos/Delete/5
        public ActionResult Delete(int? id)
        {
            TempData["item"] = "avaliacao";//Serve para voltar na mesma posição na pagina do trabalho
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Avaliacao avaliacao = db.Avalia_Trabalho.Find(id);
            if (avaliacao == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(avaliacao);
        }

        // POST: Avaliacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["item"] = "avaliacao";//Serve para voltar na mesma posição na pagina do trabalho
            var trabalho_temp = TempData["Trabalho"] as Trabalho;
            TempData["Trabalho"] = trabalho_temp;
            Avaliacao avaliacao = db.Avalia_Trabalho.Find(id);
            try
            {
                db.Avalia_Trabalho.Remove(avaliacao);
                db.SaveChanges();
                return RedirectToAction("../Trabalhoes/ViewTrabalho1");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, tente novamente mais tarde";
                return View(avaliacao);
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
