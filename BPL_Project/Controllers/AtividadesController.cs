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
    public class AtividadesController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

        

        // GET: Atividades/Create
        public ActionResult Create()
        {
            TempData["item"] = "atividade";//Serve para voltar na mesma posição na pagina do trabalho
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            Atividade atividade = new Atividade { };
            atividade.Data_Exec= trabalho_temp.Data_Tranca;
            return View(atividade);
        }

        // POST: Atividades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Atividade_Id,Descricao,Data_Exec,Trabalho_Id")] Atividade atividade)
        {
            TempData["item"] = "atividade";//Serve para voltar na mesma posição na pagina do trabalho
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                atividade.Prof_Id = prof.Prof_Id;
                atividade.Trabalho_Id = trabalho_temp.Trabalho_Id;
                if (atividade.Data_Exec < trabalho_temp.Data_Tranca)
                {
                    ViewData["data_tranca"] = "Data de execução não pode ser anterior a data de execução do trabalho ("+ trabalho_temp.Data_Tranca.ToString("dd/MM/yyyy") + ")";
                    return View(atividade);
                }
                else if (atividade.Data_Exec >= trabalho_temp.Data_Fim)
                {
                    ViewData["data_tranca"] = "Data de execução não pode ser posterior ou igual a data fim (" + trabalho_temp.Data_Fim.ToString("dd/MM/yyyy") + ")";
                    return View(atividade);
                }
                try
                {
                    db.Atividades.Add(atividade);
                    db.SaveChanges();
                    TempData["Message"] = "Cadastrado com sucesso";
                    return RedirectToAction("Create");
                }
                catch (Exception) {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(atividade);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(atividade);
        }

        // GET: Atividades/Edit/5
        public ActionResult Edit(int? id)
        {
            TempData["item"] = "atividade";
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Atividade atividade = db.Atividades.Find(id);
            if (atividade == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(atividade);
        }

        // POST: Atividades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Atividade_Id,Descricao,Data_Exec,Trabalho_Id")] Atividade atividade)
        {
            TempData["item"] = "atividade";
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                atividade.Prof_Id = prof.Prof_Id;
                atividade.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Entry(atividade).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(atividade);
                }
                
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(atividade);
        }

        // GET: Atividades/Delete/5
        public ActionResult Delete(int? id)
        {
            TempData["item"] = "atividade";
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Atividade atividade = db.Atividades.Find(id);
            if (atividade == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(atividade);
        }

        // POST: Atividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["item"] = "atividade";
            var trabalho_temp = TempData["Trabalho"] as Trabalho;
            TempData["Trabalho"] = trabalho_temp;
            Atividade atividade = db.Atividades.Find(id);
            try
            {
                db.Atividades.Remove(atividade);
                db.SaveChanges();
                return RedirectToAction("../Trabalhoes/ViewTrabalho1");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, tente novamente mais tarde";
                return View(atividade);
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
