using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Usuarios = new List<object>();
            usuario.Rol.Roles = new List<object>();
            ML.Result resultList = BL.Rol.GetAll();
            if (resultList.Correct)
            {
                usuario.Rol.Roles = resultList.Objects;
            }
            else
            {
                ViewBag.Message = resultList.ErrorMessage;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                var responseTask = client.GetAsync("Usuario");
                responseTask.Wait();
                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultUsuario in readTask.Result.Objects)
                    {
                        ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultUsuario.ToString());
                        usuario.Usuarios.Add(resultItemList);
                    }
                }

            }

            return View(usuario);
        }
        [HttpGet]
        public ActionResult Form(int? IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Usuarios = new List<object>();
            usuario.Rol = new ML.Rol();
            usuario.Rol.Roles = new List<object>();
            ML.Result resultRol = BL.Rol.GetAll();


            if (IdUsuario != null)//Update
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                    var responseTask = client.GetAsync("Usuario/" + IdUsuario);
                    responseTask.Wait();
                    var resultService = responseTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        ML.Usuario resultItemUsuario = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                        usuario = resultItemUsuario;

                    }
                }
                usuario.Rol.Roles = resultRol.Objects;
            }
            else //Add
            {
                usuario.Rol.Roles = resultRol.Objects;

            }
            return View(usuario);
        }
        [HttpPost]
        public ActionResult Form(ML.Usuario usuario, string password)
        {
            if (usuario.IdUsuario == 0) //Add
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                    var postTask = client.PostAsJsonAsync("Usuario", usuario);
                    postTask.Wait();

                    var resultService = postTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se agrego correctamente el usuario";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo agregar el usuario";
                    }
                }
            }
            else//Update
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                    var putTask = client.PutAsJsonAsync("Usuario/" + usuario.IdUsuario, usuario);
                    putTask.Wait();
                    var resultPut = putTask.Result;
                    if (resultPut.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se actualizo el usuario";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo actualizar el usuario";
                    }
                }
            }
            return PartialView("Modal");
        }
        public ActionResult Delete(int IdUsuario)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                var deleteTask = client.DeleteAsync("Usuario/" + IdUsuario);
                deleteTask.Wait();
                var resultService = deleteTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se elimino correctamente el usuario";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo eliminar el usuario";
                }
            }
            return PartialView("Modal");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Usuario"); 
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registro(ML.Usuario usuario, string password)
        {
            ML.Result result = BL.Usuario.Registro(usuario);
            if(result.Correct)
            {
                ViewBag.Login = true;
                ViewBag.Mensaje = "Se registro correctamente el usuario";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Login = true;
                ViewBag.Mensaje = "Hubo un error al registrar el usuario";
                return PartialView("Modal");
            }
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            byte[] inputBytes=Encoding.UTF8.GetBytes(password);

            ML.Result result = BL.Usuario.UsuarioGetByEmail(email);
            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                Session["IdUsuario"] = usuario.IdUsuario;
                Session["NombreRol"] = usuario.Rol.Nombre;
                byte[] storedPassword2 = Encoding.UTF8.GetBytes(usuario.Password);

                if (inputBytes.SequenceEqual(storedPassword2)) 
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Login = true;
                    ViewBag.Mensaje = "La contraseña es incorrecta";
                    return PartialView("Modal");
                }
            }
            else
            {
                ViewBag.Login = true;
                ViewBag.Mensaje = "El email es incorrecto";
                return PartialView("Modal");
            }
        }
    }
}