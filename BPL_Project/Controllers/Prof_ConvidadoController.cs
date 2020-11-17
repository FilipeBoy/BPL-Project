using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPL_Project.Models;
using System.Data.SqlClient;

namespace BPL_Project.Controllers
{
    public class Prof_ConvidadoController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

        
        
        // GET: Prof_Convidado/Create
        public ActionResult Create()
        {
            TempData["item"] = "convidado";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            Professor prof = Session["prof"] as Professor;//busca professor logado
            ViewBag.Permissao = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = string.Empty,Value = ""},
                    new SelectListItem { Selected = false, Text = "Visualizar",Value = "Visualizar"},
                    new SelectListItem { Selected = false, Text = "Editar",Value = "Editar"},
                }, "Value", "Text");
            var profs = new List<Professor>();
            profs.AddRange(db.Professors.Where(x=>x.Prof_Id!=prof.Prof_Id).ToList());
            var profconv = db.Prof_Convidado.Where(x => x.Trabalho_Id == trabalho_temp.Trabalho_Id).ToList();
            
            if (profconv != null)
            {
                foreach (var item in profconv)
                {
                    var professor = db.Professors.Find(item.Prof_Id);
                    profs.Remove(professor);
                }
            }
            
            ViewBag.Prof_Id = new SelectList(profs.OrderBy(x => x.Nome).ToList(), "Prof_Id", "Nome");
            return View();
        }

        // POST: Prof_Convidado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Prof_Conv_Id,Prof_Id,Permissao,Trabalho_Id")] Prof_Convidado prof_Convidado)
        {
            TempData["item"] = "convidado";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            var prof = Session["prof"] as Professor;// Busca os dados do professor que está logado
            if (ModelState.IsValid)
            {
                prof_Convidado.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Prof_Convidado.Add(prof_Convidado);
                    db.SaveChanges();
                    TempData["Message"] = "Enviado convite com sucesso";
                    return RedirectToAction("Create");
                }
                catch (Exception )
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    if (prof_Convidado.Permissao.Equals("Visualizar"))
                    {
                        ViewBag.Permissao = new SelectList(
                        new List<SelectListItem>
                        {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Editar",Value = "Editar"},
                        }, "Value", "Text");
                    }
                    else if (prof_Convidado.Permissao.Equals("Editar"))
                    {
                        ViewBag.Permissao = new SelectList(
                       new List<SelectListItem>
                       {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Visualizar",Value = "Visualizar"},
                       }, "Value", "Text");
                    }

                    var profList = new List<Professor>();
                    profList.AddRange(db.Professors.Where(x => x.Prof_Id != prof.Prof_Id));
                    var conv = db.Prof_Convidado.Where(x => x.Trabalho_Id == trabalho_temp.Trabalho_Id);
                    if (conv != null)
                    {
                        foreach (var item in conv)
                        {
                            profList.Remove(db.Professors.Where(x => x.Prof_Id == item.Prof_Id).FirstOrDefault());
                        }
                    }
                    profList.OrderBy(x => x.Nome);
                    ViewBag.Prof_Id = new SelectList(profList, "Prof_Id", "Nome");
                    return View(prof_Convidado);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            if (prof_Convidado.Permissao.Equals("Visualizar"))
            {
                ViewBag.Permissao = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Editar",Value = "Editar"},
                }, "Value", "Text");
            }
            else if (prof_Convidado.Permissao.Equals("Editar"))
            {
                ViewBag.Permissao = new SelectList(
               new List<SelectListItem>
               {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Visualizar",Value = "Visualizar"},
               }, "Value", "Text");
            }

            var profs = new List<Professor>();
            profs.AddRange(db.Professors.Where(x => x.Prof_Id != prof.Prof_Id));
            var profconv = db.Prof_Convidado.Where(x => x.Trabalho_Id == trabalho_temp.Trabalho_Id);
            if (profconv != null)
            {
                foreach (var item in profconv)
                {
                    profs.Remove(db.Professors.Where(x => x.Prof_Id == item.Prof_Id).FirstOrDefault());
                }
            }
            profs.OrderBy(x => x.Nome);
            ViewBag.Prof_Id = new SelectList(profs, "Prof_Id", "Nome");
            return View(prof_Convidado);
        }

        // GET: Prof_Convidado/Edit/5
        public ActionResult Edit(int? id)
        {
            TempData["item"] = "convidado";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            Professor prof = Session["prof"] as Professor;//busca professor logado
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Prof_Convidado prof_Convidado = db.Prof_Convidado.Find(id);
            if (prof_Convidado == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            if (prof_Convidado.Permissao.Equals("Visualizar"))
            {
                ViewBag.Permissao = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Editar",Value = "Editar"},
                }, "Value", "Text");
            }
            else if (prof_Convidado.Permissao.Equals("Editar"))
            {
                ViewBag.Permissao = new SelectList(
               new List<SelectListItem>
               {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Visualizar",Value = "Visualizar"},
               }, "Value", "Text");
            }
            ViewBag.Prof_Id = new SelectList((from p in db.Professors
                                              where p.Prof_Id == prof_Convidado.Prof_Id
                                              select p

                           ), "Prof_Id", "Nome");
            return View(prof_Convidado);
        }

        // POST: Prof_Convidado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Prof_Conv_Id,Prof_Id,Permissao,Trabalho_Id")] Prof_Convidado prof_Convidado)
        {
            TempData["item"] = "convidado";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            Professor prof = Session["User"] as Professor;//busca professor logado
            if (ModelState.IsValid)
            {
                var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
                TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
                prof_Convidado.Trabalho_Id = trabalho_temp.Trabalho_Id;
                try
                {
                    db.Entry(prof_Convidado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Trabalhoes/ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    if (prof_Convidado.Permissao.Equals("Visualizar"))
                    {
                        ViewBag.Permissao = new SelectList(
                        new List<SelectListItem>
                        {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Editar",Value = "Editar"},
                        }, "Value", "Text");
                    }
                    else if (prof_Convidado.Permissao.Equals("Editar"))
                    {
                        ViewBag.Permissao = new SelectList(
                       new List<SelectListItem>
                       {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Visualizar",Value = "Visualizar"},
                       }, "Value", "Text");
                    }
                    ViewBag.Prof_Id = new SelectList((from p in db.Professors
                                                      where p.Prof_Id == prof_Convidado.Prof_Id
                                                      select p

                                   ), "Prof_Id", "Nome");
                    return View(prof_Convidado);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            if (prof_Convidado.Permissao.Equals("Visualizar"))
            {
                ViewBag.Permissao = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Editar",Value = "Editar"},
                }, "Value", "Text");
            }
            else if (prof_Convidado.Permissao.Equals("Editar"))
            {
                ViewBag.Permissao = new SelectList(
               new List<SelectListItem>
               {
                    new SelectListItem { Selected = true, Text = prof_Convidado.Permissao,Value = prof_Convidado.Permissao},
                    new SelectListItem { Selected = false, Text = "Visualizar",Value = "Visualizar"},
               }, "Value", "Text");
            }
            ViewBag.Prof_Id = new SelectList((from p in db.Professors
                                              where p.Prof_Id == prof_Convidado.Prof_Id
                                              select p

                           ), "Prof_Id", "Nome");
            return View(prof_Convidado);
        }

        // GET: Prof_Convidado/Delete/5
        public ActionResult Delete(int? id)
        {
            TempData["item"] = "convidado";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Prof_Convidado prof_Convidado = db.Prof_Convidado.Find(id);
            if (prof_Convidado == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(prof_Convidado);
        }

        // POST: Prof_Convidado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["item"] = "convidado";//Serve para voltar na mesma posição na pagina ViewTrabalho1
            var trabalho_temp = TempData["Trabalho"] as Trabalho;// Busca o trabalho que está em aberto
            TempData["Trabalho"] = trabalho_temp;// Mantém o trabalho aberto
            Prof_Convidado prof_Convidado = db.Prof_Convidado.Find(id);
            db.Prof_Convidado.Remove(prof_Convidado);
            var atividades = db.Atividades.Where(x => x.Trabalho_Id.Equals(trabalho_temp.Trabalho_Id) && x.Prof_Id.Equals(prof_Convidado.Prof_Id));
            foreach(var item in atividades)
            {
                db.Atividades.Remove(item);
            }
            var objetivos = db.Objetivoes.Where(x => x.Trabalho_Id.Equals(trabalho_temp.Trabalho_Id) && x.Prof_Id.Equals(prof_Convidado.Prof_Id));
            foreach (var item in objetivos)
            {
                db.Objetivoes.Remove(item);
            }
            var avaliacao = db.Avalia_Trabalho.Where(x => x.Trabalho_Id.Equals(trabalho_temp.Trabalho_Id) && x.Prof_Id.Equals(prof_Convidado.Prof_Id));
            foreach (var item in avaliacao)
            {
                db.Avalia_Trabalho.Remove(item);
            }
            var produtos = db.Produtoes.Where(x => x.Trabalho_Id.Equals(trabalho_temp.Trabalho_Id) && x.Prof_Id.Equals(prof_Convidado.Prof_Id));
            foreach (var item in produtos)
            {
                db.Produtoes.Remove(item);
            }
            try
            {
                db.SaveChanges();
                return RedirectToAction("../Trabalhoes/ViewTrabalho1");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, por favor tente mais tarde";
                return View(prof_Convidado);
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
