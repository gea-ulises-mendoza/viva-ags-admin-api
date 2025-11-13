

using System.Collections.Generic;

namespace VivaAguascalientesAPI.DAO.Model
{
    public class RedesModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Prefijo { get; set; }
        
        public static IList<RedesModel> Get()
        {
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<RedesModel, object>("[dbo].[sp_AT_ObtenRedes]", null);
            return result;
        }
    }
    
    
}