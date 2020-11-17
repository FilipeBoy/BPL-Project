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
    public class CursoesController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

        // GET: Cursoes
        public ActionResult Index()
        {
            return View(db.Cursoes.ToList());
        }

        // GET: Cursoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(curso);
        }

        // GET: Cursoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cursoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Curso_Id,Nome")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Cursoes.Add(curso);
                    db.SaveChanges();
                    TempData["Message"] = "Cadastrado com sucesso";
                    return RedirectToAction("Create");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(curso);
                }
                
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(curso);
        }

        // GET: Cursoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(curso);
        }

        // POST: Cursoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Curso_Id,Nome")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(curso).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(curso);
                }
                
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(curso);
        }

        // GET: Cursoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Curso curso = db.Cursoes.Find(id);
            if (curso == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(curso);
        }

        // POST: Cursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Cursoes.Find(id);
            try
            {
                db.Cursoes.Remove(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, tente novamente mais tarde";
                return View(curso);
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
