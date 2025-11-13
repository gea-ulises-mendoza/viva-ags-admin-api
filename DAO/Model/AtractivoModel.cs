using System;
using System.Collections.Generic;
using VivaAguascalientesAPI.DTO.RequestDTO;

namespace VivaAguascalientesAPI.DAO.Model
{
    public class AtractivoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Horarios { get; set; }
        public string Costos { get; set; }
        public string Notas { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string NombreEn { get; set; }
        public string DescripcionEn { get; set; }
        public string DireccionEn { get; set; }
        public string HorariosEn { get; set; }
        public string CostosEn { get; set; }
        public string NotasEn { get; set; }

        public static IList<AtractivoModel> Get()
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<AtractivoModel, object>("[dbo].[sp_AT_clabAtractivoLeer]", null);
            return result;
        }

        public static AtractivoModel GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<AtractivoModel, object>("[dbo].[sp_AT_clabAtractivoLeer]", new { IdAtractivo = id });
            return result;
        }

        public static GaleriaModel  GetImage(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<GaleriaModel, object>("[dbo].[sp_AT_ObtenPortadaAtractivo]", new { IdAtractivo = id });
            return result;
        }

        public static AtractivoModel Create(object data)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<InstertedValue, object>("[dbo].[sp_AT_clabAtractivoCrearTodo]", data);
            return GetById(result.Id);
        }

        /*
        public static AtractivoModel Update(UpdateAtractivoRequest data)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetModel<InstertedValue, UpdateAtractivoRequest>("[dbo].[sp_AT_clabAtractivoActualizar]", data);
            return GetById(result.Id);
        }
        */

        public static bool Delete(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.ExecuteNonQueryModel<object>("[dbo].[sp_AT_clabAtractivoBorrar]", new { IdAtractivo = id });
            if (result != 200)
            {
                return false;
            }
            return true;
        }
    }
    
    public class CategoriaAtractivoModel
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public bool Recomendado { get; set; }
        
        public static IList<CategoriaAtractivoModel> GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<CategoriaAtractivoModel, object>("[dbo].[sp_AT_ObtenCategoriasPorAtractivo]", new { IdAtractivo = id });
            return result;
        }

    }
    
    public class RedesSocialesModel
    {
        public int Id { get; set; }
        public int IdEnlace { get; set; }
        public string Liga { get; set; }
        
        public static IList<RedesSocialesModel> GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<RedesSocialesModel, object>("[dbo].[sp_AT_ObtenEnlacesPorAtractivo]", new { IdAtractivo = id });
            return result;
        }

    }

    public class TelefonoModel
    {
        public int Id { get; set; }
        public long Telefono { get; set; }
        public int? Extension { get; set; }
        
        public static IList<TelefonoModel> GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<TelefonoModel, object>("[dbo].[sp_AT_ObtenTelefonos]", new { IdAtractivo = id });
            return result;
        }

    }
    
    public class GaleriaModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Ruta { get; set; }
        public Boolean Portada { get; set; }

        public static IList<GaleriaModel> GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<GaleriaModel, object>("[dbo].[sp_AT_ObtenFotosGaleriaPorAtractivo]", new { IdAtractivo = id });
            return result;
        }
    }

    public class EtiquetaModel
    {
        public int Id { get; set; }
        public string Etiqueta { get; set; }

        public static IList<EtiquetaModel> Get()
        {
            var dataAccess = new DataAccessModelService();
            var result =
                dataAccess.GetListModel<EtiquetaModel, object>("[dbo].[sp_AT_ObtenEtiquetas]", null);
            return result;
        }
        
        public static IList<EtiquetaModel> GetById(int id)
        {
            var dataAccess = new DataAccessModelService();
            var result =
                dataAccess.GetListModel<EtiquetaModel, object>("[dbo].[sp_AT_ObtenEtiquetasPorAtractivo]", new { IdAtractivo = id });
            return result;
        }
    }
    
}
