using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Inventario2.Models;
using System.IO;

namespace Inventario2.Controllers
{
    
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.proveedor.ToList());
            }

        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventarioEntities())
            {
                proveedor objProveedor = db.proveedor.Where(a => a.id == id).FirstOrDefault();
                return View(objProveedor);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(proveedor proveedor)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    proveedor objProveedor = db.proveedor.Find(proveedor.id);
                    objProveedor.nombre = proveedor.nombre;
                    objProveedor.direccion = proveedor.direccion;
                    objProveedor.telefono = proveedor.telefono;
                    objProveedor.nombre_contacto = proveedor.nombre_contacto;
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
        public ActionResult Create(proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventarioEntities())
                {
                    db.proveedor.Add(proveedor);
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

        public ActionResult mostrarReporte()
        {
            var db = new inventarioEntities();
            //proveedor objProveedor = db.proveedor.Where(a => a.id == 1).FirstOrDefault();
            //var usuario = (from p in db.producto select new { p.nombre }).ToList();
            //ViewData["datos"] = usuario;
            var query = from proveedor in db.proveedor
                        join producto in db.producto on proveedor.id equals producto.id_proveedor
                        select new ProdProve{ proveedor = proveedor.nombre, producto = producto.nombre };
            

            return View(query);
            
            
        }

        public ActionResult PaginadorIndex(int pagina = 1)
        {
            var cantidadRegistrosPagina = 5;
            using (var db = new inventarioEntities())
            {
                var proveedores = db.proveedor.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistrosPagina)
                    .Take(cantidadRegistrosPagina).ToList();

                var totalRegistros = db.usuario.Count();
                var modelo = new ProveedorIndex();
                modelo.Proveedores = proveedores;
                modelo.ActualPage = pagina;
                modelo.Total = totalRegistros;
                modelo.RecordsPage = cantidadRegistrosPagina;
                modelo.ValoresQueryString = new RouteValueDictionary();

                return View(modelo);
            }
        }

        public ActionResult FormImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormImage(HttpPostedFileBase fileForm)
        {
            string filePath = string.Empty;
            if(fileForm != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(fileForm.FileName);
                string extension = Path.GetExtension(fileForm.FileName);
                fileForm.SaveAs(filePath);
            }
            return View();
        }

        public ActionResult UploadCSV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadCSV(HttpPostedFileBase fileForm)
        {
            string filePath = string.Empty;
            if (fileForm != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(fileForm.FileName);
                string extension = Path.GetExtension(fileForm.FileName);
                fileForm.SaveAs(filePath);

                string csvData = System.IO.File.ReadAllText(filePath);
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var newProveedor = new proveedor
                        {
                            nombre = row.Split(';')[0],
                            nombre_contacto = row.Split(';')[1],
                            direccion = row.Split(';')[2],
                            telefono = row.Split(';')[3]
                        };

                        using (var db = new inventarioEntities())
                        {
                            db.proveedor.Add(newProveedor);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return View();
        }
    }
}