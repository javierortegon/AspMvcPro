using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario2.Controllers;
using Inventario2.Models;

namespace Inventario2.Filters
{
    public class VerificarSesion: ActionFilterAttribute
    {
        private usuario ousuario;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                ousuario = (usuario)HttpContext.Current.Session["User"];
                if (ousuario == null)
                {
                    if (filterContext.Controller is UsuarioController == false)
                    {

                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Usuario/Login");
            }
            
        }
    }
}