namespace VivaAguascalientesAPI.DTO.RequestDTO
{

    public class CreateVideoRequest
    {
        public string Nombre { get; set; }
        public byte[] Video { get; set; }
    }
}