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
    public class ProdutoesController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

       

        // GET: Produtoes/Create
        public ActionResult Create()
        {
            TempData["item"] = "produto";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            return View();
        }

        // POST: Produtoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Produto_Id,Descricao,Trabalho_Id")] Produto produto)
        {
            TempData["item"] = "produto";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                produto.Prof_Id = prof.Prof_Id;
                produto.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Produtoes.Add(produto);
                    db.SaveChanges();
                    TempData["Message"] = "Cadastrado com sucesso";
                    return RedirectToAction("Create");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(produto);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(produto);
        }

        // GET: Produtoes/Edit/5
        public ActionResult Edit(int? id)
        {
            TempData["item"] = "produto";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Produto produto = db.Produtoes.Find(id);
            if (produto == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Produto_Id,Descricao,Trabalho_Id")] Produto produto)
        {
            TempData["item"] = "produto";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                produto.Prof_Id = prof.Prof_Id;
                produto.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Entry(produto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(produto);
                }
                
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(produto);
        }

        // GET: Produtoes/Delete/5
        public ActionResult Delete(int? id)
        {
            TempData["item"] = "produto";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Produto produto = db.Produtoes.Find(id);
            if (produto == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["item"] = "produto";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            Produto produto = db.Produtoes.Find(id);
            try
            {
                db.Produtoes.Remove(produto);
                db.SaveChanges();
                return RedirectToAction("../Trabalhoes/ViewTrabalho1");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, tente novamente mais tarde";
                return View(produto);
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
