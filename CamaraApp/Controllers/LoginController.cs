using CamaraApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamaraApp.Controllers
{
    public class LoginController : Controller
    {
        DBEntities DB = new DBEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Validate(Usuario model)
        {
            var user = DB.Usuario.FirstOrDefault(x => x.Correo == model.Correo && x.Contraseña == model.Contraseña);

            if (user == null)
            {
                model.ErrorMessage = "Datos inválidos";
                return View("Index", model);
            }
            else
            {
                Session["login"] = model.UsuarioId;
                return RedirectToAction("Index", "Camara");
            }
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Usuario model)
        {
            DB.Usuario.Add(model);
            DB.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}