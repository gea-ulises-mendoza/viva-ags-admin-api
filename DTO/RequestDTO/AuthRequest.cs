using System;
namespace VivaAguascalientesAPI.DTO.RequestDTO
{
    public class AuthRequest
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }

    public class ForgetAuthRequest
    {
        public string Usuario { get; set; }
    }

    public class ResetAuthRequest
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ResetCode { get; set; }
    }
}
