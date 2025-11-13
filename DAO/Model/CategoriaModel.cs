using System;
using System.Collections.Generic;
using VivaAguascalientesAPI.DTO.RequestDTO;

namespace VivaAguascalientesAPI.DAO.Model
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Pin { get; set; }
        public string Imagen { get; set; }
        public string Color { get; set; }

        public string Tag { get; set; }
        public string Descripcion { get; set; }

        public int? Padre{ get; set; }
        public int Orden { get; set; }
        public string Icono { get; set; }
        public bool Activo { get; set; }

        public string Introduccion { get; set; }

        public string NombreEn { get; set; }

        public string DescripcionEn { get; set; }

        public string IntroduccionEn { get; set; }

        public bool Principal { get; set; }


        public static IList<CategoriaModel> Get()
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<CategoriaModel, object>("[dbo].[sp_AT_clabCategorias]", null);
            return result;
        }

        public static CategoriaModel GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<CategoriaModel, object>("[dbo].[sp_AT_clabCategoriaLeer]", new { IdCategoria = id });
            return result;
        }

        public static IList<CategoriaModel> getSubcategory(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<CategoriaModel, object>("[dbo].[sp_AT_clabCategorias]", new { IdCategoria = id });
            return result;
        }

        public static CategoriaModel Create(CreateCatgoriaReuqest data)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<InstertedValue, object>(
                "[dbo].[sp_AT_clabCategoriaCrear]",
                new
                {
                    Nombre = data.Nombre,
                    Descripcion = data.Descripcion,
                    Padre = data.Padre,
                    Color = data.Color?.Replace("#",""),
                    Tag = data.Tag,
                    NombreEn = data.NombreEn,
                    DescripcionEn = data.DescripcionEn,
                    Introduccion = data.Introduccion,
                    IntroduccionEn = data.IntroduccionEn,
                    Principal = data.Principal
                });
            return GetById(result.Id);
        }

        public static CategoriaModel Update(UpdateCatgoriaReuqest data)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<InstertedValue, object>("[dbo].[sp_AT_clabCategoriaActualizar]", new
            {
                IdCategoria = data.IdCategoria,
                Nombre = data.Nombre,
                Descripcion = data.Descripcion, 
                NombreEn = data.NombreEn,
                DescripcionEn = data.DescripcionEn,
                Introduccion = data.Introduccion,
                IntroduccionEn = data.IntroduccionEn,
                Padre = data.Padre,
                Color = data.Color?.Replace("#",""),
                Tag = data.Tag,
                Principal = data.Principal
            });
            return GetById(result.Id);
        }

        public static bool Delete(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.ExecuteNonQueryModel<object>("[dbo].[sp_AT_clabCategoriaBorrar]", new { IdCategoria = id });
            if (result != 200)
            {
                return false;
            }
            return true;
        }
        
        public static int UpdatePosition(UpdateOrdenRequest data)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.ExecuteNonQueryModel<object>("[dbo].[sp_AT_clabCategoriasOrdenar]", new
            {
                IdCategoria = data.IdCategoria, 
                Orden = data.Orden
            });
            return result;
        }
    }
}