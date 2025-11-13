using System;
namespace VivaAguascalientesAPI.DTO.RequestDTO
{
	public class CreateCatgoriaReuqest
	{
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public string NombreEn { get; set; }
		public string DescripcionEn { get; set; }
		public string Introduccion { get; set; }
		public string IntroduccionEn { get; set; }
		public int? Padre { get; set; }
		public byte[] Icono { get; set; }
		public byte[] Imagen { get; set; }
		public byte[] Pin { get; set; }
		public string Color { get; set; }
		public string Tag { get; set; }
		public bool Principal { get; set; }
	}

	public class UpdateCatgoriaReuqest : CreateCatgoriaReuqest
	{
		public int IdCategoria { get; set; }
	}

	public class UpdateOrdenRequest
	{
		public int IdCategoria { get; set; }
		public int Orden { get; set; }
	}
}