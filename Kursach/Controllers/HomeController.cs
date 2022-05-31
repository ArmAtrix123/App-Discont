using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kursach.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Kursach.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationContex db;
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(ApplicationContex contex, IHttpContextAccessor httpContextAccessor)
        {
            db = contex;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(int? id, string login, int page = 1, StateSort sortOrder = StateSort.IdAsc)
        {
            IQueryable<User> users = db.Users;
            //Фильтрация или поиск
            if (id != null && id > 0)
            {
                users = users.Where(p => p.Id_user == id);
            }
            if (!String.IsNullOrEmpty(login))
            {
                users = users.Where(p => p.Email.Contains(login));
            }
            //Сортировка
            switch (sortOrder)
            {
                case StateSort.IdAsc:
                    {
                        users = users.OrderBy(m => m.Id_user);
                        break;
                    }
                case StateSort.IdDesc:
                    {
                        users = users.OrderByDescending(m => m.Id_user);
                        break;
                    }
                case StateSort.EmailAsc:
                    {
                        users = users.OrderBy(m => m.Email);
                        break;
                    }
                case StateSort.EmailDesc:
                    {
                        users = users.OrderByDescending(m => m.Email);
                        break;
                    }
            }
            //Пагинация
            int pageSize = 5;
            var count = await users.CountAsync();
            var item = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            IndexViewModel indexViewModel = new IndexViewModel
            {
                FilterViewModel = new FilterViewModel(id, login),
                SortViewModel = new SortViewModel(sortOrder),
                PageViewModel = new PageViewModel(count, page, pageSize),
                Users = item

            };

            return View(indexViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Glavnaya()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult Avtorization()
        {
            return View();
        }
        public IActionResult NewPost()
        {
            return View();
        }
        public async Task<IActionResult> GlavnayaUser()
        {
            IQueryable<Modeli> post = db.Modelis;
            return View(await post.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Post(Modeli modeli)
        {
            IQueryable<Modeli> post = db.Modelis;

            return View(await post.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id != null)
            {
                Modeli modeli = await db.Modelis.FirstOrDefaultAsync(predicate => predicate.ID_Model == id);
                if (modeli != null)
                    return View(modeli);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (user.Login == "adm" && user.Password == "adm")
            {
                return RedirectToAction("Index");
            }
            if (user.Login == null || user.Password == null || user.Ima == null || user.Familia == null || user.Email == null || user.DataR == null)
            {
                return RedirectToAction("Index");
            }
            if (user.Otchestvo == null) user.Otchestvo = " ";

            User use = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == user.Login && predicate.Email == user.Email);

            if (use != null)
            {
                //Ошибка, пользователь уже существует в бд
                return RedirectToAction("Index");
            }
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        public async Task<IActionResult> DetaliPosta(int? id)
        {
            if (id != null)
            {
                Modeli modeli = await db.Modelis.FirstOrDefaultAsync(predicate => predicate.ID_Model == id);
                if (modeli != null)
                    return View(modeli);
            }
            return NotFound();
        }
        public async Task<IActionResult> Registrations(User user)
        {

            if (user.Login == "adm" && user.Password == "adm")
            {
                return RedirectToAction("Error");
            }
            if (user.Login == null || user.Password == null || user.Ima == null || user.Familia == null || user.Email == null || user.DataR == null)
            {
                return RedirectToAction("Error");
            }
            if (user.Otchestvo == null) user.Otchestvo = " ";

            User use = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == user.Login && predicate.Email == user.Email);

            if (use != null)
            {
                return RedirectToAction("Error");
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Glavnaya");
        }
        public async Task<IActionResult> Avtoriz(LoginUser user)
        {
            if (user.login == null || user.password == null) return RedirectToAction("Error");
            if (user.login == "adm" && user.password == "adm") return RedirectToAction("Index");
           
                User use = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == user.login && predicate.Password == user.password);
            if (use == null)
            {
                return RedirectToAction("Error");
            }
            else if (user.login == use.Login && user.password == use.Password)
            {
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Append("user", Convert.ToString(use.Id_user), cookie); //Создание Cookie-файла
                    return RedirectToAction("GlavnayaUser");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> NewPosti(Modeli modeli)
        {
            if (modeli.Model == null || modeli.skidki == null || modeli.picture == null || modeli.info == null) return RedirectToAction("Post");
            Modeli fto = await db.Modelis.FirstOrDefaultAsync(predicate => predicate.Model == modeli.Model);
            if (fto != null)
            {
                return RedirectToAction("Post");
            }
            db.Modelis.Add(modeli);
            await db.SaveChangesAsync();
            return RedirectToAction("Post");
        }
        [HttpPost]
        public async Task<IActionResult> EditPost(Modeli modeli)
        {
            db.Modelis.Update(modeli);
            await db.SaveChangesAsync();
            return RedirectToAction("Post");
        }
        public async Task<IActionResult> EditStrUser()
        {
            int? id = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Cookies["user"]);
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (id != null)
                    return View(user);
            }
            return NotFound();
        }
        public async Task<IActionResult> Edituser(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edituser(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("GlavnayaUser");
        }
        [HttpGet]
        [ActionName("DeletePost")]
        public async Task<IActionResult> DelitPost(int? id)
        {
            if (id != null)
            {
                Modeli modeli = await db.Modelis.FirstOrDefaultAsync(predicate => predicate.ID_Model == id);
                if (modeli != null)
                    return View(modeli);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id != null)
            {
                Modeli modeli = await db.Modelis.FirstOrDefaultAsync(predicate => predicate.ID_Model == id);
                if (modeli != null)
                {
                    db.Modelis.Remove(modeli);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Post");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> DetaliStrUser()
        {
            int? id = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Cookies["user"]);
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id_user == id);
                if (id != null)
                    return View(user);
            }
            return NotFound();
        }
    }
}
