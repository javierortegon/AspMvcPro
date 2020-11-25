using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario2.Models;
using Rotativa;
using Inventario2.Filtros;

namespace Inventario2.Controllers
{
    
    public class ProductoController : Controller
    {
        [AuthorizeUser(idRol:3)]
        // GET: Producto
        public ActionResult Index()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.producto.ToList());
            }
        }
        
        public ActionResult listarProveedores()
        {
            using (var db = new inventarioEntities())
            {
                return PartialView(db.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(producto producto)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventarioEntities())
                {
                    db.producto.Add(producto);
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

        public static string NombreProveedor(int? idProveedor)
        {
            using (var db = new inventarioEntities())
            {
                return db.proveedor.Find(idProveedor).nombre;
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventarioEntities())
            {
                producto producto = db.producto.Where(a => a.id == id).FirstOrDefault();
                return View(producto);
            }
        }

        public ActionResult VistaCustom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VistaCustom(FormCollection formCollection)
        {
            string name = formCollection["user"];
            string pass = formCollection["password"];
            return View();
        }

        //metodo que crea la vista
        public ActionResult Reporte()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.producto.ToList());
            }
        }

        public ActionResult ImprimirReporte()
        {
            return new ActionAsPdf("Reporte") { FileName = "Productos.pdf" };
        }

    }
}