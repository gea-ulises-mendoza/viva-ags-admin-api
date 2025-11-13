using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivaAguascalientesAPI.Core;
using VivaAguascalientesAPI.DAO;
using VivaAguascalientesAPI.DAO.Model;
using VivaAguascalientesAPI.DTO.RequestDTO;
using VivaAguascalientesAPI.DTO.ResponseDTO;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VivaAguascalientesAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController, Produces("application/json")]
    [Authorize]
    public class CategoriasController : Controller
    {
        /// <summary>
        /// Recupera todas las cateorías en el sistema
        /// </summary>
        /// <response code="200">Lista de categorias</response>
        [HttpGet]
        [ProducesResponseType(typeof(IList<CategoriaModel>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            var dao = CategoriaModel.Get();
            return Ok(dao);
        }

        /// <summary>
        /// Recupera la información de una categoría
        /// </summary>
        /// <param name="id">Id de la categoría a recuperar su información</param>
        /// <response code="200">Lista de categorias</response>
        [HttpGet("{id}", Name = nameof(Get))]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Get(int id)
        {
            var dao = CategoriaModel.GetById(id);
            if (dao == null)
            {
                return NotFound();
            }
            var subcategory = CategoriaModel.getSubcategory(id);
            CategoriaResponse result = new CategoriaResponse(dao);
            result.Hijos = subcategory;

            return Ok(result);
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        /// <param name="request">Datos a insertar en la base de datos</param>
        /// <response code="201">Se creo recurso correctamente</response>
        /// <response code="400">Petición incorrecta</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoriaModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] CreateCatgoriaReuqest request)
        {
            CategoriaModel dao = CategoriaModel.Create(request);
            if (dao == null)
            {
                return BadRequest();
            }

            if (request.Imagen != null)
            {
                System.IO.File.WriteAllBytes("image/categoria/portada/" + dao.Id, request.Imagen);
            }
            
            if (request.Icono != null)
            {
                System.IO.File.WriteAllBytes("image/categoria/icono/" + dao.Id, request.Icono);
            }
            
            if (request.Pin != null)
            {
                System.IO.File.WriteAllBytes("image/categoria/pin/" + dao.Id, request.Pin);
            }

            return CreatedAtAction(
                nameof(Get),
                ControllerContext.ActionDescriptor.ControllerName,
                new
                {
                    id = dao.Id
                },
                dao);
        }

        /// <summary>
        /// Modifica los datos de una categoria
        /// </summary>
        /// <param name="request">Datos a insertar en la base de datos</param>
        /// <param name="id">Identificador del recurso a modificar</param>
        /// <response code="200">Se creo recurso correctamente</response>
        /// <response code="400">Petición incorrecta</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] UpdateCatgoriaReuqest request)
        {
            var dao = CategoriaModel.Update(request);
            if (dao == null)
            {
                return BadRequest();
            }
            if (request.Imagen != null)
            {
                System.IO.File.WriteAllBytes("image/categoria/portada/" + dao.Id, request.Imagen);
            }

            if (request.Icono != null)
            {
                System.IO.File.WriteAllBytes("image/categoria/icono/" + dao.Id, request.Icono);
            }

            if (request.Pin != null)
            {
                System.IO.File.WriteAllBytes("image/categoria/pin/" + dao.Id, request.Pin);
            }
            return NoContent();
        }

        /// <summary>
        /// Elimina un registro de la BD
        /// </summary>
        /// <param name="id">Identificador del recurso a eliminar</param>
        /// <response code="200">Se elimino el recurso correctamente</response>
        /// <response code="400">Petición incorrecta</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {

            var result = CategoriaModel.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }
        
        /// <summary>
        /// Recupera las cateorías aptas para contener atractivos,
        /// es decir, que no sean categorías de primer nivel y
        /// que no sean padres de otras categorías
        /// </summary>
        /// <response code="200">Lista de categorias</response>
        [HttpGet]
        [Route("AltaAtractivo")]
        [ProducesResponseType(typeof(IList<CategoriaModel>), StatusCodes.Status200OK)]
        public ActionResult GetCategorias()
        {
            var dao = CategoriaAltaAtractivoModel.Get();
            return Ok(dao);
        }
        
        /// <summary>
        /// Modifica la posición de una categoría
        /// </summary>
        /// <param name="request">Id y posición final de la categoría</param>
        /// <response code="200">Se creo recurso correctamente</response>
        /// <response code="400">Petición incorrecta</response>
       // [HttpPut("Orden/{id:int}/{orden:int}")]
        [HttpPut]
        [Route("Orden")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePosition([FromBody] UpdateOrdenRequest request)
        {
            var result = CategoriaModel.UpdatePosition(request);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }

            //if (dao == null)
            // {
            //    return BadRequest();
            // }
            // return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("icono/{id:long}/{size:int}/{h:int}")]
        [HttpGet("icono/{id:long}/{size:int}")]
        [HttpGet("icono/{id:long}")]
        [Produces("application/image")]
        public ActionResult IconoImage(int id, int size = 150, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage("categoria/icono/", "" + id, size, h);

            return File(image, "img/png");
        }

        [AllowAnonymous]
        [HttpGet("icono/b64/{id:long}/{size:int}/{h:int}")]
        [HttpGet("icono/b64/{id:long}/{size:int}")]
        [HttpGet("icono/b64/{id:long}")]
        public ActionResult IconoImage64(int id, int size = 500, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage64("categoria/icono/", "" + id, size, h);

            return Ok(image);
        }

        [AllowAnonymous]
        [HttpGet("pin/{id:long}/{size:int}/{h:int}")]
        [HttpGet("pin/{id:long}/{size:int}")]
        [HttpGet("pin/{id:long}")]
        [Produces("application/image")]
        public ActionResult PinImage(int id, int size = 30, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage("categoria/pin/", "" + id, size, h);

            return File(image, "img/png");
        }

        [AllowAnonymous]
        [HttpGet("pin/b64/{id:long}/{size:int}/{h:int}")]
        [HttpGet("pin/b64/{id:long}/{size:int}")]
        [HttpGet("pin/b64/{id:long}")]
        public ActionResult PinImage64(int id, int size = 500, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage64("categoria/pin/", "" + id, size, h);

            return Ok(image);
        }

        [AllowAnonymous]
        [HttpGet("portada/{id:long}/{size:int}/{h:int}")]
        [HttpGet("portada/{id:long}/{size:int}")]
        [HttpGet("portada/{id:long}")]
        [Produces("application/image")]
        public ActionResult PortadaImage(int id, int size = 500, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage("categoria/portada/", "" + id, size, h);

            return File(image, "img/png");
        }

        [AllowAnonymous]
        [HttpGet("portada/b64/{id:long}/{size:int}/{h:int}")]
        [HttpGet("portada/b64/{id:long}/{size:int}")]
        [HttpGet("portada/b64/{id:long}")]
        public ActionResult PortadaImage64(int id, int size = 500, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage64("categoria/portada/", "" + id, size, h);

            return Ok(image);
        }
    }
}
