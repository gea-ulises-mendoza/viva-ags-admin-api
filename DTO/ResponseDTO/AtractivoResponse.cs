using System.Collections.Generic;
using Newtonsoft.Json;
using VivaAguascalientesAPI.DAO.Model;

namespace VivaAguascalientesAPI.DTO.ResponseDTO
{
    public class AtractivoResponse : AtractivoModel
    {
        public AtractivoResponse(AtractivoModel model)
        {
            Id = model.Id;
            Nombre = model.Nombre;
            Descripcion = model.Descripcion;
            Direccion = model.Direccion;
            Horarios = model.Horarios;
            Costos = model.Costos;
            Notas = model.Notas;
            Latitud = model.Latitud;
            Longitud = model.Longitud;
            NombreEn = model.NombreEn;
            DescripcionEn = model.DescripcionEn;
            DireccionEn = model.DireccionEn;
            HorariosEn = model.HorariosEn;
            CostosEn = model.CostosEn;
            NotasEn = model.NotasEn;

        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<GaleriaModel> ListaFotos { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<TelefonoModel> ListaTelefonos { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<RedesSocialesModel> ListaEnlaces { get; set; } 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<CategoriaAtractivoModel> ListaCategorias { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<EtiquetaModel> ListaEtiquetas { get; set; }
    }
}