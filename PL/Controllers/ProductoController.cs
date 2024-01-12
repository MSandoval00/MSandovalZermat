using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Producto producto=new ML.Producto();
            producto.Descuento=new ML.Descuento();
            producto.Categoria=new ML.Categoria();
            producto.Productos = new List<object>();
            producto.Descuento.Descuentos = new List<object>();
            producto.Categoria.Categorias=new List<object>();
            ML.Result resultCategoria = BL.Categoria.GetAll();
            if (resultCategoria.Correct)
            {
                producto.Categoria.Categorias = resultCategoria.Objects;
            }
            else
            {
                ViewBag.Mensaje = resultCategoria.ErrorMessage;
            }
            ML.Result resultDescuentos = BL.Descuento.GetAll();
            if (resultDescuentos.Correct)
            {
                producto.Descuento.Descuentos = resultDescuentos.Objects;
            }
            else
            {
                ViewBag.Mensaje=resultDescuentos.ErrorMessage;
            }
            using (var client =new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                var responseTask = client.GetAsync("Producto");
                responseTask.Wait();

                var resultService=responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultProducto in readTask.Result.Objects)
                    {
                        ML.Producto resultItemProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(resultProducto.ToString());
                        producto.Productos.Add(resultItemProducto);
                    }
                }
            }
            return View(producto);
        }
        [HttpGet]
        public ActionResult Form(int? Sku)
        {
            ML.Producto producto = new ML.Producto();
            producto.Descuento = new ML.Descuento();
            producto.Categoria = new ML.Categoria();
            producto.Productos = new List<object>();
            producto.Descuento.Descuentos = new List<object>();
            producto.Categoria.Categorias = new List<object>();
            ML.Result resultCategoria = BL.Categoria.GetAll();
            ML.Result resultDescuentos = BL.Descuento.GetAll();
            
            if (Sku!=null)//Update
            {
                producto.Imagen = Session["ImagenProducto"] as string;
                if (producto.Imagen == null)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                        var responseTask = client.GetAsync("Producto/" + Sku);
                        responseTask.Wait();
                        var resultService = responseTask.Result;
                        if (resultService.IsSuccessStatusCode)
                        {
                            var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();

                            ML.Producto resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(readTask.Result.Object.ToString());
                            producto = resultItemList;
                        }
                    }
                }
                Session["ImagenProducto"] = producto.Imagen;
                producto.Categoria.Categorias = resultCategoria.Objects;
                producto.Descuento.Descuentos = resultDescuentos.Objects;
            }
            else//Add
            {
                producto.Categoria.Categorias = resultCategoria.Objects;
                producto.Descuento.Descuentos = resultDescuentos.Objects;
            }
            return View(producto);
        }
        [HttpPost]
        public ActionResult Form(ML.Producto producto, HttpPostedFileBase Imagen)
        {
            producto.Imagen= Session["ImagenProducto"] as string;

            if (Imagen != null && Imagen.ContentLength > 0)
            {
                producto.Imagen = ConvertirBase64(Imagen);
                Session["ImagenProducto"] = producto.Imagen;
            }
            if (producto.Sku == 0)//Add
            {
                using (var client=new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                    var postTask = client.PostAsJsonAsync("Producto",producto);
                    postTask.Wait();
                    var resultService=postTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se agrego correctamente el producto";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo agregar el producto al catalogo";
                    }
                }
            }
            else//Update
            {
                using (var client=new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                    var putTask = client.PutAsJsonAsync("Producto/" + producto.Sku, producto);
                    putTask.Wait();
                    var resultPut = putTask.Result;
                    if (resultPut.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se actualizó el producto correctamente";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo actualizar el producto correctamente";
                    }
                }
                Session.Remove("ImagenProducto");

            }
            return PartialView("Modal");
        }
        public ActionResult Delete(int Sku)
        {
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["API"]);
                var deleteTask = client.DeleteAsync("Producto/" + Sku);
                deleteTask.Wait();
                var resultService = deleteTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se elimino correctamente el producto";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo eliminar el producto correctamente";
                }
            }
            return PartialView("Modal");
        }
        public string ConvertirBase64(HttpPostedFileBase Foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(Foto.InputStream);
            byte[] data = reader.ReadBytes((int)Foto.ContentLength);
            string Imagen = Convert.ToBase64String(data);
            return Imagen;
        }
        public ActionResult AddProductos(int Sku)
        {
            bool existe =false;
            ML.Venta venta = new ML.Venta();
            venta.ListaProductos = new List<object>();
            ML.Result result = BL.Producto.GetById(Sku);
            if (Session["ListaProductos"]==null)
            {
                if (result.Correct)
                {
                    ML.Producto producto = (ML.Producto)result.Object;
                    producto.Piezas = 1;
                    venta.ListaProductos.Add(producto);
                    Session["ListaProductos"] = Newtonsoft.Json.JsonConvert.SerializeObject(venta.ListaProductos);
                }

            }
            else
            {
                ML.Producto producto = (ML.Producto)result.Object;
                GetCarrito(venta);
                foreach (ML.Producto item in venta.ListaProductos)
                {
                    if (producto.Sku==item.Sku)
                    {
                        item.Piezas += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe==true)
                {
                    Session["ListaProductos"] = Newtonsoft.Json.JsonConvert.SerializeObject(venta.ListaProductos);
                }
                else
                {
                    producto.Piezas = 1;
                    venta.ListaProductos.Add(producto);
                    Session["ListaProductos"] = Newtonsoft.Json.JsonConvert.SerializeObject(venta.ListaProductos);
                }
            }
            return RedirectToAction("Productos");
        }
        [HttpGet]
        public ActionResult Productos()
        {
            ML.Venta venta= new ML.Venta();
            venta.ListaProductos = new List<object>();
            if ( Session["ListaProductos"] ==null)
            {
                return View(venta);
            }
            else
            {
                GetCarrito(venta);
                return View(venta);
            }
        }
        public ML.Venta GetCarrito(ML.Venta venta)
        {
            var lista = Session["ListaProductos"];
            var carritoSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(lista.ToString());
            foreach (var itemProducto in  carritoSession)
            {
                ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(itemProducto.ToString());
                venta.ListaProductos.Add(objProducto);
            }
            return venta;
        }
    }
}