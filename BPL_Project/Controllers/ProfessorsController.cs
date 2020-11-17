using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPL_Project.Models;
using System.Security.Cryptography;
using System.Text;


namespace BPL_Project.Controllers
{
    public class ProfessorsController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();
        

        // GET: Professors
        public ActionResult Index()
        {
            TempData["Page"] = "/Professors/Index";//guarda a pagina atual
            ViewData["Message"] = TempData["Message"];
            var professors = db.Professors.Include(p => p.Curso).OrderBy(x => x.Nome);
            return View(professors.ToList());
        }

        // GET: Professors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = TempData["Message"];
                var prof = Session["prof"] as Professor;//busca professor logado
                id = prof.Prof_Id;
            }

            Professor professor = db.Professors.Find(id);
            TempData["Professor"] = professor;
            if (professor == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(professor);
        }

        // GET: Professors/Create
        public ActionResult Create()
        {
            var curso = db.Cursoes.OrderBy(x => x.Nome);
            ViewBag.Curso_Id = new SelectList(curso, "Curso_Id", "Nome");
            return View();
        }

        // POST: Professors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Prof_Id,Nome,Email,Password,Curso_Id")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                TempData["prof"] = professor;//armazena professor criado para criar usuário
                Curso curso = db.Cursoes.Where(x => x.Curso_Id == professor.Curso_Id).FirstOrDefault();
                professor.Curso_Nome = curso.Nome;
                User user = new User();
                user.Paper = "user";
                user.Email = professor.Email;
                using (MD5 md5Hash = MD5.Create())
                {
                    user.Password = GetMd5Hash(md5Hash, professor.Password);
                }

                try
                {
                    db.Professors.Add(professor);
                    db.Users.Add(user);
                    db.SaveChanges();
                    TempData["Message"] = "Cadastrado com sucesso";
                    return RedirectToAction("Create");
                }
                catch (Exception)
                {

                }

            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde!";
            ViewBag.Curso_Id = new SelectList(db.Cursoes, "Curso_Id", "Nome", professor.Curso_Id);
            return View(professor);
        }

        // GET: Professors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido!";
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                ViewData["Message"] = "Item não encontrado!";
            }
            ViewBag.Curso_Id = new SelectList(db.Cursoes, "Curso_Id", "Nome", professor.Curso_Id);
            return View(professor);
        }

        // POST: Professors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Prof_Id,Nome,Email,Password,Curso_Id")] Professor professor)
        {
             Curso curso = db.Cursoes.Where(x => x.Curso_Id == professor.Curso_Id).FirstOrDefault();
                professor.Curso_Nome = curso.Nome;


                var trabalhos = db.Trabalhoes.Where(x => x.Prof_Id == professor.Prof_Id).ToList();
                foreach (var item in trabalhos)
                {
                    item.Prof_Curso = curso.Nome;
                }

                var User = Session["User"] as User;
            
                if (User != null)
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        User.Email = professor.Email;
                        User.Password = GetMd5Hash(md5Hash, professor.Password);
                        db.Entry(User).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    try
                    {
                        db.Entry(professor).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["Message"] = "Alterado com sucesso";
                        return RedirectToAction("Details");
                    }
                    catch (Exception)
                    {
                        ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
                        ViewBag.Curso_Id = new SelectList(db.Cursoes, "Curso_Id", "Nome", professor.Curso_Id);
                        return View(professor);
                    }
                }
                
            
            ViewData["Message"] = "Entrada inválida";
            ViewBag.Curso_Id = new SelectList(db.Cursoes, "Curso_Id", "Nome", professor.Curso_Id);
            return View(professor);
        }

        // GET: Professors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(professor);
        }

        // POST: Professors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Professor professor = db.Professors.Find(id);
            var user = db.Users.Where(x => x.Email.Equals(professor.Email)).FirstOrDefault();
            var trabalhos = db.Trabalhoes.Where(x => x.Prof_Id == professor.Prof_Id).ToList();
            foreach (var item in trabalhos)
            {

                var objetivo = db.Objetivoes.Where(x => x.Trabalho_Id.Equals(item.Trabalho_Id)).ToList();//busca todos os objetivos relacionados ao trabalho
                foreach (var item2 in objetivo)
                {
                    db.Objetivoes.Remove(item2);//remove todos os objetivos do trabalho
                }

                var atividade = db.Atividades.Where(x => x.Trabalho_Id.Equals(item.Trabalho_Id)).ToList();//busca todas as atividades do trabalho
                foreach (var item2 in atividade)
                {
                    db.Atividades.Remove(item2);//exclui todas as atividades do trabalho
                }

                var produto = db.Produtoes.Where(x => x.Trabalho_Id.Equals(item.Trabalho_Id)).ToList();//busca todos os produtos do trabalho
                foreach (var item2 in produto)
                {
                    db.Produtoes.Remove(item2);//exclui todos os produtos do trabalho
                }

                var avaliacao = db.Avalia_Trabalho.Where(x => x.Trabalho_Id.Equals(item.Trabalho_Id)).ToList();//busca todas as avaliações do trabalho
                foreach (var item2 in avaliacao)
                {
                    db.Avalia_Trabalho.Remove(item2);//exclui todas as avaliações do trabalho
                }


                var experiencia = db.Experiencias.Where(x => x.Trabalho_Id.Equals(item.Trabalho_Id)).ToList();//busca todos os convidados do trabalho
                foreach (var item2 in experiencia)
                {
                    db.Experiencias.Remove(item2);//exclui todas as experiencias do trabalho
                }
                var prof_Convidado = db.Prof_Convidado.Where(x => x.Trabalho_Id.Equals(item.Trabalho_Id)).ToList();//busca todos os convidados do trabalho
                foreach (var item2 in prof_Convidado)
                {
                    db.Prof_Convidado.Remove(item2);//exclui todos os convidados
                }

                db.Trabalhoes.Remove(item);
            }

            var objetivos = db.Objetivoes.Where(x => x.Prof_Id.Equals(professor.Prof_Id)).ToList();//busca todos os objetivos relacionados ao trabalho
            foreach (var item2 in objetivos)
            {
                db.Objetivoes.Remove(item2);//remove todos os objetivos do trabalho
            }

            var atividades = db.Atividades.Where(x => x.Prof_Id.Equals(professor.Prof_Id)).ToList();//busca todas as atividades do trabalho
            foreach (var item2 in atividades)
            {
                db.Atividades.Remove(item2);//exclui todas as atividades do trabalho
            }

            var produtos = db.Produtoes.Where(x => x.Prof_Id.Equals(professor.Prof_Id)).ToList();//busca todos os produtos do trabalho
            foreach (var item2 in produtos)
            {
                db.Produtoes.Remove(item2);//exclui todos os produtos do trabalho
            }

            var avaliacaos = db.Avalia_Trabalho.Where(x => x.Prof_Id.Equals(professor.Prof_Id)).ToList();//busca todas as avaliações do trabalho
            foreach (var item2 in avaliacaos)
            {
                db.Avalia_Trabalho.Remove(item2);//exclui todas as avaliações do trabalho
            }

            var prof_Convidados = db.Prof_Convidado.Where(x => x.Prof_Id.Equals(professor.Prof_Id)).ToList();//busca todos os convidados do trabalho
            foreach (var item2 in prof_Convidados)
            {
                db.Prof_Convidado.Remove(item2);//exclui todos os convidados
            }

            var experiencias = db.Experiencias.Where(x => x.Prof_Id.Equals(professor.Prof_Id)).ToList();//busca todos os convidados do trabalho
            foreach (var item2 in experiencias)
            {
                db.Experiencias.Remove(item2);//exclui todas as experiencias do trabalho
            }

            try
            {
                db.Users.Remove(user);
                db.Professors.Remove(professor);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Message"] = "Não foi possível excluir, tente mais tarde";
                return View(professor);
            }
            TempData["Message"] = "Professor excluído com sucesso";
            return RedirectToAction("Index");

        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
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
