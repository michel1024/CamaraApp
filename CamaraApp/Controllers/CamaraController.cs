using CamaraApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CamaraApp.Controllers
{
    public class CamaraController : Controller
    {
        DBEntities DB = new DBEntities();

        // GET: Camara
        public ActionResult Index()
        {
            var list = DB.Camara.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Camara model)
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var imagen = System.Web.HttpContext.Current.Request.Files["Imagen"];
                var path = Server.MapPath("~/Content/img/");
                var name = imagen.FileName;

                imagen.SaveAs(path + name);

                model.Imagen = "~/Content/img/" + imagen.FileName;

                DB.Camara.Add(model);
                DB.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            var camara = DB.Camara.FirstOrDefault(x => x.CamaraId == id);
            return View(camara);
        }

        [HttpGet]
        public ActionResult Actualizar(int id)
        {
            var camara = DB.Camara.FirstOrDefault(x => x.CamaraId == id);
            return View(camara);
        }

        [HttpPost]
        public ActionResult Actualizar(Camara model)
        {
            var actualizar = DB.Camara.FirstOrDefault(x => x.CamaraId == model.CamaraId);

            actualizar.Fabricante = model.Fabricante;
            actualizar.Serial = model.Serial;
            actualizar.Descripcion = model.Descripcion;

            if (System.Web.HttpContext.Current.Request.Files["Imagen"].FileName == "")
            {
                DB.SaveChanges();
            }else if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var imagen = System.Web.HttpContext.Current.Request.Files["Imagen"];
                var path = Server.MapPath("~/Content/img/");
                var name = imagen.FileName;

                imagen.SaveAs(path + name);
                actualizar.Imagen = "~/Content/img/" + imagen.FileName;
                
                DB.SaveChanges();
            }

            return RedirectToAction("Detalle", new { id = model.CamaraId });
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var delete = DB.Camara.FirstOrDefault(x => x.CamaraId == id);
            DB.Camara.Remove(delete);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SendEmail(string nombre, string correo, string asunto, string mensaje)
        {
            string cuerpoCorreo = "<h1>"+nombre+"</h1><br />"+"<b>Correo:</b> "+correo+ "<br /><br />"+mensaje;

            // Estancia da Classe de Mensagem
            MailMessage _mailMessage = new MailMessage();
            // Remetente
            //_mailMessage.To.Add("");
            _mailMessage.From = new MailAddress("erick1024@hotmail.com");

            // Destinatario seta no metodo abaixo

            //Contrói o MailMessage
            _mailMessage.CC.Add("erick.g.michel.eg@gmail.com");
            _mailMessage.Subject = asunto;
            _mailMessage.IsBodyHtml = true;
            _mailMessage.Body = cuerpoCorreo;

            //CONFIGURAÇÃO COM PORTA
            SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

            //CONFIGURAÇÃO SEM PORTA
            // SmtpClient _smtpClient = new SmtpClient(UtilRsource.ConfigSmtp);

            // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential("erik.garam@gmail.com", "Francia.98");

            _smtpClient.EnableSsl = true;

            _smtpClient.Send(_mailMessage);

            return RedirectToAction("Index", "Camara");
        }
    }
}