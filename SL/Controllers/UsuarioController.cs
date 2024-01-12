using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL.Controllers
{
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            ML.Result result = BL.Usuario.GetAll();
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
        [Route("{IdUsuario}")]
        public IHttpActionResult GetById(int IdUsuario)
        {
            ML.Result result = BL.Usuario.GetById(IdUsuario);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
        [HttpPost]
        [Route("")]
        public IHttpActionResult Add([FromBody]ML.Usuario usuario)
        {
            ML.Result result=BL.Usuario.Add(usuario);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
        [HttpPut]
        [Route("{IdUsuario}")]
        public IHttpActionResult Update([FromBody]ML.Usuario usuario, int IdUsuario)
        {
            usuario.IdUsuario=IdUsuario;
            ML.Result result = BL.Usuario.Update(usuario);
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
        [Route("{IdUsuario}")]
        public IHttpActionResult Delete(int IdUsuario)
        {
            ML.Result result = BL.Usuario.Delete(IdUsuario);
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
