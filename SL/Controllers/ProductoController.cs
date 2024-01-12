using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL.Controllers
{
    [RoutePrefix("api/Producto")]
    public class ProductoController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            ML.Result result=BL.Producto.GetAll();
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK,result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
        [HttpPost]
        [Route("")]
        public IHttpActionResult Add([FromBody]ML.Producto producto)
        {
            ML.Result result=BL.Producto.Add(producto);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK,result);
            }
            else
            {
                return Content(HttpStatusCode.OK, result);
            }
        }
        [HttpPut]
        [Route("{Sku}")]
        public IHttpActionResult Update([FromBody]ML.Producto producto, int Sku)
        {
            producto.Sku = Sku;
            ML.Result result = BL.Producto.Update(producto);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
        [HttpDelete]
        [Route("{Sku}")]
        public IHttpActionResult Delete(int Sku)
        {
            ML.Result result = BL.Producto.Delete(Sku);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
        [HttpGet]
        [Route("{Sku}")]
        public IHttpActionResult GetById(int Sku)
        {
            ML.Result result = BL.Producto.GetById(Sku);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
    }
}
