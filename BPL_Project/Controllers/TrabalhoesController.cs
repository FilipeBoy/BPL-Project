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
    public class TrabalhoesController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();

        // GET: Trabalhoes
        public ActionResult AdminIndex()
        {
            return View(db.Trabalhoes.ToList());
        }

        // GET: Trabalhoes
        public ActionResult Index()
        {
            TempData["item"] = null;//limpa item do trabalho
            TempData["Trabalho"] = null;//limpa trabalho aberto
            TempData["Permissao"] = null;//limpa permissão do trabalho
            var prof = Session["prof"] as Professor;//busca professor logado
            TempData["Prof_Id"] = prof.Prof_Id;//armazena código do professor logado
            var trabalhos = new List<Trabalho>();
            var tempTrabalhos = db.Trabalhoes.Where(x => x.Prof_Id.Equals(prof.Prof_Id)).ToList();
            if (tempTrabalhos != null)
            {
                trabalhos.AddRange(tempTrabalhos);
            }
            var Trab_convidado = db.Prof_Convidado.Where(x => x.Prof_Id.Equals(prof.Prof_Id)).ToList();
            if (Trab_convidado != null)
            {
                foreach (var item in Trab_convidado)
                {
                    var trabalho = db.Trabalhoes.Find(item.Trabalho_Id);
                    trabalhos.Add(trabalho);
                }
            }
            foreach (var item2 in trabalhos)
            {
                int alterado = 0;
                if (item2.Status != "Concluído")
                {
                    if (DateTime.Today.Date >= item2.Data_Fim)
                    {
                        item2.Status = "Concluído";
                        alterado = 1;
                    }
                    else if (DateTime.Today.Date >= item2.Data_Tranca)
                    {
                        item2.Status = "Andamento";
                        alterado = 1;
                    }
                }
                if (alterado != 0)
                {
                    db.Entry(item2).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return View(trabalhos.ToList());
        }


        // GET: Trabalhoes/Create
        public ActionResult Create()
        {
            var prof = Session["prof"] as Professor;//busca professor logado
            Trabalho trabalho = new Trabalho();
            trabalho.Prof_Id = prof.Prof_Id;
            trabalho.Data_Inicio = DateTime.Today.Date;
            trabalho.Data_Tranca = DateTime.Today.Date.AddDays(1);
            trabalho.Data_Fim = DateTime.Today.Date.AddDays(2);
            return View(trabalho);
        }

        // POST: Trabalhoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Trabalho_Id,Assunto,Problema,Data_Inicio,Data_Tranca,Data_Fim,Status")] Trabalho trabalho)
        {
            if (ModelState.IsValid)
            {
                var prof = Session["prof"] as Professor;//busca professor logado
                trabalho.Prof_Id = prof.Prof_Id;
                Curso curso = db.Cursoes.Where(x => x.Curso_Id == prof.Curso_Id).FirstOrDefault();
                trabalho.Prof_Curso = curso.Nome;
                trabalho.Prof_Nome = prof.Nome;
                trabalho.Status = "Planejamento";
                //validação das datas
                if(trabalho.Data_Inicio< DateTime.Today.Date)
                {
                    ViewData["data_inicio"] = "Data de início não pode ser anterior a data atual";
                    return View(trabalho);
                }else if (trabalho.Data_Tranca <= trabalho.Data_Inicio)
                {
                    ViewData["data_tranca"] = "Data de execução não pode ser anterior ou igual a data de início";
                    return View(trabalho);
                }else if (trabalho.Data_Tranca >= trabalho.Data_Fim)
                {
                    ViewData["data_tranca"] = "Data de execução não pode ser posterior ou igual a data fim";
                    return View(trabalho);
                }
                else if (trabalho.Data_Fim <= trabalho.Data_Tranca)
                {
                    ViewData["data_fim"] = "Data fim não pode ser anterior ou igual a data de execução";
                    return View(trabalho);
                }
                try
                {
                    db.Trabalhoes.Add(trabalho);
                    db.SaveChanges();
                    TempData["Permissao"] = "Editar";//dá permissão de editor para o autor do trabalho
                    TempData["Trabalho"] = trabalho;//guarda trabalho aberto
                    return RedirectToAction("ViewTrabalho1");
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                    return View(trabalho);
                }
                
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(trabalho);
        }

        // GET: Trabalhoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Trabalho trabalho = db.Trabalhoes.Find(id);
            if (trabalho == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            TempData["Trabalho"] = trabalho;
            return View(trabalho);
        }

        // POST: Trabalhoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Trabalho_Id,Assunto,Data_Inicio,Data_Tranca,Data_Fim,Status,Prof_Id")] Trabalho trabalho)
        {
            if (ModelState.IsValid)
            {
                Trabalho trabalhoTemp = TempData["Trabalho"] as Trabalho;//busca trabalho aberto
                TempData["Trabalho"] = trabalhoTemp;//guarda trabalho aberto
                trabalho.Problema = trabalhoTemp.Problema;
                trabalho.Prof_Curso = trabalhoTemp.Prof_Curso;
                trabalho.Prof_Nome = trabalhoTemp.Prof_Nome;
                trabalho.Prof_Id = trabalhoTemp.Prof_Id;
                trabalho.Status = trabalhoTemp.Status;
                
                try
                {
                    db.Entry(trabalho).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = e;
                    return View(trabalho);
                }
                TempData["Trabalho"] = trabalho;
                return RedirectToAction("ViewTrabalho1");
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(trabalho);
        }

        //--------------------------------------------------
        // GET: Trabalhoes/Edit/5
        public ActionResult EditProblem(int? id)//edição somente do problema do trabalho
        {
            TempData["item"] = "problema";
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Trabalho trabalho = db.Trabalhoes.Find(id);
            if (trabalho == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            TempData["Trabalho"] = trabalho;
            return View(trabalho);
        }

        // POST: Trabalhoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProblem([Bind(Include = "Trabalho_Id,Problema")] Trabalho trabalho)
        {
            TempData["item"] = "problema";// serve para voltar ao item do problema na ViewTrabalho1
            if (ModelState.IsValid)
            {
                Trabalho trabalhoTemp = TempData["Trabalho"] as Trabalho;//busca trabalho aberto
                TempData["Trabalho"] = trabalhoTemp;//guarda trabalho aberto
                trabalho.Prof_Id = trabalhoTemp.Prof_Id;
                trabalho.Status = trabalhoTemp.Status;
                trabalho.Assunto = trabalhoTemp.Assunto;
                trabalho.Data_Inicio = trabalhoTemp.Data_Inicio;
                trabalho.Data_Tranca = trabalhoTemp.Data_Tranca;
                trabalho.Data_Fim = trabalhoTemp.Data_Fim;
                trabalho.Prof_Curso = trabalhoTemp.Prof_Curso;
                trabalho.Prof_Nome = trabalhoTemp.Prof_Nome;
                
                try
                {
                    db.Entry(trabalho).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
                    return View(trabalho);
                }
                TempData["Trabalho"] = trabalho;
                return RedirectToAction("ViewTrabalho1");
            }
            ViewData["Message"] = "Não foi possível alterar, tente novamente mais tarde";
            return View(trabalho);
        }
        //---------------------------------------------------
        // GET: Trabalhoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Trabalho trabalho = db.Trabalhoes.Find(id);
            if (trabalho == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(trabalho);
        }

        // POST: Trabalhoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trabalho trabalho = db.Trabalhoes.Find(id);
            

            var objetivo = db.Objetivoes.Where(x => x.Trabalho_Id.Equals(id));//busca todos os objetivos relacionados ao trabalho
            foreach (var item in objetivo)
            {
                db.Objetivoes.Remove(item);//remove todos os objetivos do trabalho
            }

            var atividade = db.Atividades.Where(x => x.Trabalho_Id.Equals(id));//busca todas as atividades do trabalho
            foreach (var item in atividade)
            {
                db.Atividades.Remove(item);//exclui todas as atividades do trabalho
            }

            var produto = db.Produtoes.Where(x => x.Trabalho_Id.Equals(id));//busca todos os produtos do trabalho
            foreach (var item in produto)
            {
                db.Produtoes.Remove(item);//exclui todos os produtos do trabalho
            }

            var avaliacao = db.Avalia_Trabalho.Where(x => x.Trabalho_Id.Equals(id));//busca todas as avaliações do trabalho
            foreach (var item in avaliacao)
            {
                db.Avalia_Trabalho.Remove(item);//exclui todas as avaliações do trabalho
            }

            var prof_Convidado = db.Prof_Convidado.Where(x => x.Trabalho_Id.Equals(id));//busca todos os convidados do trabalho
            foreach (var item in prof_Convidado)
            {
                db.Prof_Convidado.Remove(item);//exclui todos os convidados
            }

            var experiencias = db.Experiencias.Where(x => x.Trabalho_Id.Equals(id));//busca todos os convidados do trabalho
            foreach (var item in experiencias)
            {
                db.Experiencias.Remove(item);//exclui todas as experiencias do trabalho
            }
            try
            {
                db.Trabalhoes.Remove(trabalho);
                db.SaveChanges();
                if (Session["Paper"].Equals("admin"))
                {
                    return RedirectToAction("AdminIndex");
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewData["Message"] = "Não foi possível excluir, por favor tente mais tarde";
            }
            
            if (Session["Paper"].Equals("admin"))
            {
                return RedirectToAction("AdminIndex");
            }
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: trabalhoes/ViewTrabalho1

        public ActionResult ViewTrabalho1(int? id)
        {
            if (TempData["item"] != null)//se está vindo de alguma edição de item do trabalho
            {
                string page = TempData["item"].ToString();
                ViewData["item"] = page;
            }

            if (id == null)
            {
                var trabalho = TempData["Trabalho"] as Trabalho;//busca trabalho aberto
                id = trabalho.Trabalho_Id;
            }
            var trabalho_aux = db.Trabalhoes.Include(t => t.Professor).Include(t => t.Objetivo).
                Include(t => t.Atividade).Include(t => t.Produto).Include(t => t.Avaliacao).
                Include(t => t.Prof_Convidado).Where(x => x.Trabalho_Id == id).FirstOrDefault();
            if (trabalho_aux != null)
            {
                trabalho_aux.Atividade = trabalho_aux.Atividade.OrderBy(t => t.Data_Exec).ToList();
                TempData["Trabalho"] = trabalho_aux;
                var prof = Session["prof"] as Professor;
                TempData["Prof_Id"] = prof.Prof_Id;
                if (trabalho_aux.Status.Equals("Andamento") || trabalho_aux.Status.Equals("Concluído"))
                {
                    TempData["Status"] = "Bloqueado";//bloqueia a edição do trabalho
                    if (trabalho_aux.Status.Equals("Concluído"))
                    {
                        var experienciaUser = db.Experiencias.Where(x => x.Prof_Id == prof.Prof_Id).FirstOrDefault();
                        if (experienciaUser != null)
                        {
                            ViewData["Experience"] = 0;
                        }
                    }
                }
                var prof_conv = trabalho_aux.Prof_Convidado;
                if (prof.Prof_Id.Equals(trabalho_aux.Prof_Id))
                {
                    TempData["Permissao"] = "Editar";
                }
                else
                {
                    foreach (var subitem in prof_conv)
                    {
                        if (subitem.Prof_Id.Equals(prof.Prof_Id))
                        {
                            TempData["Permissao"] = subitem.Permissao;
                        }
                    }
                }
            }
        return View(trabalho_aux);
        }

        // GET: trabalhoes/ViewTrabalho1

        public ActionResult ViewTrabalho2(int id)
        {

            var trabalho_aux = db.Trabalhoes.Include(t => t.Professor).Include(t => t.Objetivo).
                Include(t => t.Atividade).Include(t => t.Produto).Include(t => t.Avaliacao).
                Include(t => t.Prof_Convidado).Where(x => x.Trabalho_Id.Equals(id));
            if (trabalho_aux == null)
            {
                return HttpNotFound();
            }
            return View(trabalho_aux.ToList());
        }

        // GET: trabalhoes/ViewTrabalho1

        public ActionResult ViewExperiencias()
        {
            var prof = Session["prof"] as Professor;//busca professor logado
            Trabalho trabalhoTemp = TempData["Trabalho"] as Trabalho;//busca trabalho aberto
            TempData["Trabalho"] = trabalhoTemp;//guarda trabalho aberto
            ViewData["Titulo"] = trabalhoTemp.Assunto;
            TempData["Prof_Id"] = prof.Prof_Id;
            var experienciaUser= db.Experiencias.Where(x => x.Prof_Id == prof.Prof_Id).FirstOrDefault();
            if (experienciaUser != null)
            {
                var experiencias = db.Experiencias.Where(x => x.Trabalho_Id == trabalhoTemp.Trabalho_Id);
                return View(experiencias.ToList());
            }
            else
            {

                return RedirectToAction("ViewTrabalho1");
            }
        }

        //validações das datas

        [HttpPost]
        public JsonResult validaDataInicio (DateTime data )
        {
            var atualData = DateTime.Today.Date;

            TempData["dataInicio"] = data;
            return Json(data ==null);
        }

        [HttpPost]
        public JsonResult validaDataExec(DateTime data)
        {
            TempData["dataExec"] = data;
            if (TempData["dataInício"] != null)
            {
                var inicio = DateTime.Parse(TempData["dataInício"].ToString());
                TempData["dataInício"] = inicio;
                var atual = DateTime.Today.Date;
                return Json(data < atual && data < inicio);
            }
            var atualData = DateTime.Today.Date;


            return Json(data < atualData);
        }

        [HttpPost]
        public JsonResult validaDataConc(DateTime data)
        {
            if (TempData["dataInício"] != null)
            {
                var inicio = DateTime.Parse(TempData["dataInício"].ToString());
                TempData["dataInício"] = inicio;
                var Exec = DateTime.Parse(TempData["dataExec"].ToString());
                TempData["dataExec"] = Exec;

                var atual = DateTime.Today.Date;
                return Json(data < atual && data < inicio && data < Exec);
            }
            var atualData = DateTime.Today.Date;


            return Json(data < atualData);
        }
    
    }
}
