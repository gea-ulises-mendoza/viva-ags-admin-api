using System.Collections.Generic;

namespace VivaAguascalientesAPI.DAO.Model
{
    public class CategoriaAltaAtractivoModel
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        
        public static IList<CategoriaAltaAtractivoModel> Get()
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<CategoriaAltaAtractivoModel, object>("[dbo].[sp_AT_ObtenCategoriasAltaAtractivo]", null);
            return result;
        }
    }
    
    
}