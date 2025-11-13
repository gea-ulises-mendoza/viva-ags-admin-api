using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using VivaAguascalientesAPI.DAO.Model;

namespace VivaAguascalientesAPI.DTO.ResponseDTO
{
    public class CategoriaResponse : CategoriaModel
    {
        public CategoriaResponse(CategoriaModel model)
        {
            this.Id = model.Id;
            this.Nombre = model.Nombre;
            this.NombreEn = model.NombreEn;
            this.Pin = model.Pin;
            this.Imagen = model.Imagen;
            this.Color = model.Color;
            this.Descripcion = model.Descripcion;
            this.DescripcionEn = model.DescripcionEn;
            this.Introduccion = model.Introduccion;
            this.IntroduccionEn = model.IntroduccionEn;
            
            this.Tag = model.Tag;

            this.Padre = model.Padre;
            this.Orden = model.Orden;
            this.Icono = model.Icono;
            this.Activo = model.Activo;
            this.Principal = model.Principal;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<CategoriaModel> Hijos { get; set; }
    }
}
