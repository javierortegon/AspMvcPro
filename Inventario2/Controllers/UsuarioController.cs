using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI.WebControls;
using Inventario2.Models;
using Rotativa;


namespace Inventario2.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.usuario.ToList());
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventarioEntities())
            {
                usuario objUsuario = db.usuario.Where(a => a.id == id).FirstOrDefault();
                return View(objUsuario);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(usuario usuario)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    usuario objUsuario = db.usuario.Find(usuario.id);
                    objUsuario.nombre = usuario.nombre;
                    objUsuario.apellido = usuario.apellido;
                    objUsuario.email = usuario.email;
                    objUsuario.password = usuario.password;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(usuario usuario)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventarioEntities())
                {
                    db.usuario.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
                throw;
            }
        }

        public ActionResult Login(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            using (var db = new inventarioEntities())
            {
                var userLogin = db.usuario.FirstOrDefault(e => e.email == user && e.password == password);
                if (userLogin != null)
                {
                    FormsAuthentication.SetAuthCookie(userLogin.email, true);
                    return RedirectToAction("Index", "Proveedor");
                }
                else
                {

                    return Login("Verifique sus datos");
                }
            }
        }

        public ActionResult reporte()
        {

            using (var db = new inventarioEntities())
            {

                return View(db.usuario.ToList());
            }
        }

        public ActionResult print()
        {
            return new ActionAsPdf("reporte")
            { FileName = "Test.pdf" };
        }

        public ActionResult PaginatorIndex(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 5; // parámetro
            using (var db = new inventarioEntities())
            {
                var usuarios = db.usuario.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistrosPorPagina)
                    .Take(cantidadRegistrosPorPagina).ToList();

                var totalRegistros = db.usuario.Count();
                var modelo = new IndexModel();
                modelo.Personas = usuarios;
                modelo.ActualPage = pagina;
                modelo.Total = totalRegistros;
                modelo.RecordsPage = cantidadRegistrosPorPagina;
                modelo.ValoresQueryString = new RouteValueDictionary();

                return View(modelo);
            }
        }
    }
}