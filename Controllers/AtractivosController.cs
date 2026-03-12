using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivaAguascalientesAPI.Core;
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
    public class AtractivosController : Controller
    {
        /// <summary>
        /// Recupera todas los atractivos en el sistema
        /// </summary>
        /// <response code="200">Lista de atractivos</response>
        [HttpGet]
        [ProducesResponseType(typeof(IList<AtractivoModel>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            var dao = AtractivoModel.Get();
            return Ok(dao);
        }

        /// <summary>
        /// Recupera la información de un atractivo
        /// </summary>
        /// <param name="id">Id del atractivo a recuperar su información</param>
        /// <response code="200">Atractivo</response>
        /// <response code="404">Atractivo no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AtractivoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult AtracivoGetById(int id)
        {
            var dao = AtractivoModel.GetById(id);
            if (dao == null)
            {
                return NotFound();
            }
            AtractivoResponse response = new AtractivoResponse(dao);
            response.ListaFotos = GaleriaModel.GetById(dao.Id);
            response.ListaTelefonos = TelefonoModel.GetById(dao.Id);
            response.ListaEnlaces = RedesSocialesModel.GetById(dao.Id);
            response.ListaCategorias = CategoriaAtractivoModel.GetById(dao.Id);
            response.ListaEtiquetas = EtiquetaModel.GetById(dao.Id);
            response.ListaMunicipios = MunicipioAtractivoModel.GetById(dao.Id);
            return Ok(response);
        }

        /// <summary>
        /// Crea un nuevo atractivo
        /// </summary>
        /// <param name="request">Datos a insertar en la base de datos</param>
        /// <response code="201">Se creo recurso correctamente</response>
        /// <response code="400">Petición incorrecta</response>
        [HttpPost]
        [ProducesResponseType(typeof(AtractivoModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody] CreateAtractivoRequest request)
        {
            var result = AtractivoModel.Create(new {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Direccion = request.Direccion,
                Horarios = request.Horarios,
                Costos = request.Costos,
                Notas = request.Notas,
                NombreEn = request.NombreEn,
                DescripcionEn = request.DescripcionEn,
                DireccionEn = request.DireccionEn,
                HorariosEn = request.HorariosEn,
                CostosEn = request.CostosEn,
                NotasEn = request.NotasEn,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                ListaTelefonos = TelefonoRequest.ToDataTable(request.ListaTelefonos),
                ListaEnlaces = EnlaceRequest.ToDataTable(request.ListaEnlaces),
                ListaFotos = FotosRequest.ToDataTable(request.ListaFotos),
                ListaCategorias = AtractivoCategoriaRequest.ToDataTable(request.ListaCategorias),
                ListaEtiquetas = EtiquetaRequest.ToDataTable(request.ListaEtiquetas),
                ListaMunicipios = MunicipioAtractivoRequest.ToDataTable(request.ListaMunicipios)
            });

            if ( result == null )
            {
                return BadRequest();
            }

            var galeria = GaleriaModel.GetById(result.Id);
            foreach(GaleriaModel imagen in galeria)
            {
                FotosRequest fotoRequest = (request.ListaFotos as List<FotosRequest>).Find(img => img.Ruta == imagen.Ruta);
                System.IO.File.WriteAllBytes("image/atractivo/galeria/" + imagen.Id, fotoRequest.Imagen);
            }

            //var version = Request.HttpContext.GetRequestedApiVersion().ToString();
            var action = CreatedAtAction(
                nameof(AtracivoGetById),
                ControllerContext.ActionDescriptor.ControllerName,
                new
                {
                    id = result.Id
                },
                result);
            return action;
        }

        /// <summary>
        /// Modifica los datos de un atractivo
        /// </summary>
        /// <param name="request">Datos para actualizar</param>
        /// <param name="id">Identificador del recurso a modificar</param>
        /// <response code="200">Se modifico recurso correctamente</response>
        /// <response code="400">Petición incorrecta</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id, [FromBody] UpdateAtractivoRequest request)
        {

            var result = AtractivoModel.Create(new
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Direccion = request.Direccion,
                Horarios = request.Horarios,
                Costos = request.Costos,
                Notas = request.Notas,
                NombreEn = request.NombreEn,
                DescripcionEn = request.DescripcionEn,
                DireccionEn = request.DireccionEn,
                HorariosEn = request.HorariosEn,
                CostosEn = request.CostosEn,
                NotasEn = request.NotasEn,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                ListaTelefonos = TelefonoRequest.ToDataTable(request.ListaTelefonos),
                ListaEnlaces = EnlaceRequest.ToDataTable(request.ListaEnlaces),
                ListaFotos = FotosRequest.ToDataTable(request.ListaFotos),
                ListaCategorias = AtractivoCategoriaRequest.ToDataTable(request.ListaCategorias),
                ListaEtiquetas = EtiquetaRequest.ToDataTable(request.ListaEtiquetas),
                ListaMunicipios = MunicipioAtractivoRequest.ToDataTable(request.ListaMunicipios)
            });

            if (result == null)
            {
                return BadRequest();
            }

            var galeria = GaleriaModel.GetById(result.Id);
            foreach (GaleriaModel imagen in galeria)
            {
                FotosRequest fotoRequest = (request.ListaFotos as List<FotosRequest>).Find(img => img.Ruta == imagen.Ruta);
                System.IO.File.WriteAllBytes("image/atractivo/galeria/" + imagen.Id, fotoRequest.Imagen);
            }

            //var version = Request.HttpContext.GetRequestedApiVersion().ToString();
            var action = CreatedAtAction(
                nameof(AtracivoGetById),
                ControllerContext.ActionDescriptor.ControllerName,
                new
                {
                    id = result.Id
                },
                result);
            return action;

            //request.IdAtractivo = id;
            //var result = AtractivoModel.Update(request);
            //if (result == null)
            //{
            //    return BadRequest();
            //}
            //return NoContent();
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
        public ActionResult Delete(int id)
        {
            var result = AtractivoModel.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }
        
        /// <summary>
        /// Recupera los tipos de redes sociales
        /// </summary>
        /// 
        /// <response code="200">Lista de redes sociales</response>
        [HttpGet]
        [Route("redes")]
        [ProducesResponseType(typeof(IList<RedesModel>), StatusCodes.Status200OK)]
        public ActionResult GetRedes()
        {
            var dao = RedesModel.Get();
            return Ok(dao);
        }

        /// <summary>
        /// Recupera el catalogo de municipios
        /// </summary>
        /// <response code="200">Lista de municipios</response>
        [HttpGet]
        [Route("municipios")]
        [ProducesResponseType(typeof(IList<MunicipioModel>), StatusCodes.Status200OK)]
        public ActionResult GetMunicipios()
        {
            var dao = MunicipioModel.Get();
            return Ok(dao);
        }

        [AllowAnonymous]
        [HttpGet("galeria/{id:long}/{size:int}/{h:int}")]
        [HttpGet("galeria/{id:long}/{size:int}")]
        [HttpGet("galeria/{id:long}")]
        [Produces("application/image")]
        public ActionResult GaleriaImage(int id, int size = 500, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage("atractivo/galeria/", "" + id, size, h);

            return File(image, "img/png");
        }
        
        [AllowAnonymous]
        [HttpGet("portada/{id:long}/{size:int}/{h:int}")]
        [HttpGet("portada/{id:long}/{size:int}")]
        [HttpGet("portada/{id:long}")]
        [Produces("application/image")]
        public ActionResult AtractivoImage(int id, int size = 500, int h = 0)
        {
            var result = AtractivoModel.GetImage(id);
            var imageCore = new ImageCore();
            var idImagen = "";
            if (result == null)
            {
                idImagen = "default";
            }
            else
            {
                idImagen = result.Id.ToString();
            }

            var image = imageCore.GetImage("atractivo/galeria/", idImagen, size, h);

            return File(image, "img/png");
        }

        [AllowAnonymous]
        [HttpGet("galeria/b64/{id:long}/{size:int}/{h:int}")]
        [HttpGet("galeria/b64/{id:long}/{size:int}")]
        [HttpGet("galeria/b64/{id:long}")]
        public ActionResult GaleriaImage64(int id, int size = 0, int h = 0)
        {
            var imageCore = new ImageCore();
            var image = imageCore.GetImage64("atractivo/galeria/", "" + id, size, h);

            return Ok(image);
        }
        
        [HttpGet]
        [Route("etiquetas")]
        [ProducesResponseType(typeof(IList<EtiquetaModel>), StatusCodes.Status200OK)]
        public ActionResult GetEtiquetas()
        {
            var dao = EtiquetaModel.Get();
            return Ok(dao);
        }
    }
}
