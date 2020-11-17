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
    public class UsersController : Controller
    {
        private BPL_ProjectContext db = new BPL_ProjectContext();


        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = TempData["Message"];
                User userTemp = db.Users.Where(x => x.Paper.Equals("admin")).FirstOrDefault();
                return View(userTemp);
            }
            User user = db.Users.Find(id);
            ViewData["Message"] = TempData["Message"];
            if (user == null)
            {
                ViewData["Message"] = "Item não encontrado!";
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var prof = TempData["prof"] as Professor;//recebe professor cadastrado
            User user = new User();
            user.Paper = "user";
            user.Email = prof.Email;
            using (MD5 md5Hash = MD5.Create())
            {
                user.Password = GetMd5Hash(md5Hash, prof.Password);
            }
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("../Professors/Index");
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Id,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Paper = "user";//dá o papel de usuario para o professor
                using (MD5 md5Hash = MD5.Create())
                {
                    user.Password = GetMd5Hash(md5Hash, user.Password);
                }
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(user);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterFirst()
        {
            var user = db.Users.Where(x => x.Paper.Equals("admin")).FirstOrDefault();//verifica se há um administrador cadastrado
            if (user == null)//se não há administrador
            {

                return View();

            }
            else
            {
                if (TempData["message"] == null)
                {
                    ViewData["Message"] = "Administrador, você já está cadastrado!";
                }
                else
                {
                    string message = TempData["Message"].ToString();
                    TempData["Message"] = message;
                    ViewData["Message2"] = message;
                    ViewData["Message"] = message;
                }

            }
            TempData["Page"] = "RegisterFirst";
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterFirst([Bind(Include = "Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //cadastra administrador com o papel de administrador  e já faz o login dele
                user.Paper = "admin";
                using (MD5 md5Hash = MD5.Create())
                {
                    user.Password = GetMd5Hash(md5Hash, user.Password);
                }
                db.Users.Add(user);
                db.SaveChanges();
                Session["Name"] = "Administrador";
                Session["Paper"] = user.Paper;
                return RedirectToAction("../Home/Inicio");
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {

            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Id,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Paper = Session["Paper"].ToString();
                using (MD5 md5Hash = MD5.Create())
                {
                    user.Password = GetMd5Hash(md5Hash, user.Password);
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["Message"] = "Não foi possível salvar, tente novamente mais tarde";
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewData["Message"] = "Item não recebido";
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                ViewData["Message"] = "Item não encontrado";
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users/
        [AllowAnonymous]
        public ActionResult Login()
        {
            var user = db.Users.Where(x => x.Paper.Equals("admin")).FirstOrDefault();//verifica se há um administrador cadastrado
            if (user == null)//se não tiver um administrador cadastrado, emcaminha para o cadastro do administraddor
            {
                return RedirectToAction("../Users/RegisterFirst");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")]User user)
        {

            if (ModelState.IsValid)
            {
                var TempUser = db.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();
                if (TempUser != null)
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        
                        if (VerifyMd5Hash(md5Hash, user.Password, TempUser.Password))
                        {
                            if (Equals(TempUser.Paper, "admin"))//se for o administrador
                            {
                                Session["Name"] = "Administrador";
                                Session["Paper"] = TempUser.Paper;
                                Session["user"] = TempUser;

                            }
                            else// se for professor
                            {
                                var prof = db.Professors.Where(U => U.Email.Equals(TempUser.Email)).FirstOrDefault();
                                string[] itemNome = prof.Nome.Split(' ');//divide o nome em nome e sobrenome
                                Session["Name"] = itemNome[0];
                                Session["Paper"] = TempUser.Paper;
                                Session["Prof"] = prof;
                                Session["user"] = TempUser;


                            }
                            return RedirectToAction("../Home/Inicio");
                        }
                        else
                        {
                            ViewData["Message"] = "Senha inválida";
                            return View();
                        }

                    }

                       
                }
                else
                {
                    ViewData["Message"] = "Email inválido";
                    return View();
                }
            }
            return View(user);
        }

        // GET: Users/
        [AllowAnonymous]
        public ActionResult MudaSenha(int? id)
        {
            if (id == null)
            {
                if (TempData["Page"] != null)
                {
                    string page = TempData["Page"].ToString();
                    TempData["Page"] = page;
                }
                if (Session["Paper"] != null)//verifica se está logado
                {
                    if (Session["Paper"].ToString().Equals("admin"))//se for administrador
                    {
                        User userTemp = db.Users.Where(x => x.Paper.Equals("admin")).FirstOrDefault();
                        if (userTemp == null)
                        {
                            ViewData["Message"] = "Item não encontrado!";
                        }
                        return View(userTemp);
                    }
                    else//se for professor
                    {
                        var prof = TempData["Professor"] as Professor;
                        User user = db.Users.Where(x => x.Email.Equals(prof.Email)).FirstOrDefault();
                        if (user == null)
                        {
                            ViewData["Message"] = "Item não encontrado";
                        }
                        return View(user);
                    }
                }
                else
                {
                    return View();
                }
            }
            else//se for o administrador resetando senha do professor
            {
                var prof = db.Professors.Find(id);
                User user = db.Users.Where(x => x.Email.Equals(prof.Email)).FirstOrDefault();
                if (user == null)
                {
                    ViewData["Message"] = "Item não encontrado";
                }

                return View(user);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult MudaSenha([Bind(Include = "User_Id,Email,Password")]User user)
        {
            if (ModelState.IsValid)
            {
                var TempUser = db.Users.Find(user.User_Id);
                if (TempUser != null)
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        TempUser.Password = GetMd5Hash(md5Hash, user.Password);
                    }
                        
                    db.Entry(TempUser).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        TempData["Message"] = "Senha alterada com sucesso";
                    }
                    catch (Exception)
                    {
                        TempData["Message"] = "Não foi possível alterar a senha";
                        return View(user);
                    }
                    if (TempUser.Paper.Equals("admin"))
                    {
                        if (TempData["Page"] != null)
                        {
                            return RedirectToAction("RegisterFirst");
                        }
                        else
                        {
                            return RedirectToAction("Details");
                        }

                    }
                    else
                    {
                        return RedirectToAction("../Professors/Index");
                    }
                }
            }
            else
            {
                var temp_user = db.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();
                if (temp_user != null)
                {
                    if (temp_user.Paper.Equals("admin"))
                    {
                        using (MD5 md5Hash = MD5.Create())
                        {
                            temp_user.Password = GetMd5Hash(md5Hash, user.Password);
                        }
                         
                        db.Entry(temp_user).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                            TempData["Message3"] = "Senha alterada com sucesso";
                            return RedirectToAction("RegisterFirst");
                        }
                        catch (Exception)
                        {
                            TempData["Message3"] = "Não foi possível alterar a senha";
                            return View(user);
                        }
                    }
                    TempData["Message3"] = "Este email não é do administrador";
                    return View(user);
                }
            }
            TempData["Message3"] = "Usuário não encontrado";
            return View(user);
        }

        public ActionResult Logoff()
        {
            Session["User"] = null;//limpa usuario
            Session["Name"] = null;//limpar nome
            Session["Paper"] = null;//limpa papel
            Session["prof"] = null;//limpa prof
            return RedirectToAction("../Home/Index");
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
