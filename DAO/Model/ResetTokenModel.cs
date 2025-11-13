using System;
using System.Collections.Generic;

namespace VivaAguascalientesAPI.DAO.Model
{
    public class ResetTokenModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public bool Activo { get; set; }
        public int Usuario { get; set; }

        public static IList<ResetTokenModel> GetTokenBy(string token)
        {
            var dataParams = new
            {
                Token = token,
            };
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<ResetTokenModel, object>("[dbo].[sp_AT_ValidarResetToken]", dataParams);
            return result;
        }
    }
}
