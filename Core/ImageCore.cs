using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace VivaAguascalientesAPI.Core
{
    public class ImageCore
    {
        public byte[] GetImage(string folder, string name, int size = 500, int h = 0)
        {
            //File fileData = null;
            var defaulImage = System.IO.File.ReadAllBytes("image/"+ folder + "default");
            byte[] eventoImagen = new byte[0];

            try
            {
                var img = System.IO.File.ReadAllBytes("image/" + folder + name);
                if (img != null)
                {
                    defaulImage = img;
                }
            }
            catch (Exception ex)
            {
                
            }
            defaulImage = resizingImage(defaulImage, size, h);
            return defaulImage;
        }

        public string GetImage64(string folder, string name, int size = 500, int h = 0)
        {
            //File fileData = null;
            var defaulImage = GetImage(folder, name, size, h);
            string base64 = Convert.ToBase64String(defaulImage);
            return base64;
        }
/*
        public static byte[] resize(byte[] imageData, int width, int height)
        {
            var stream = new MemoryStream(imageData);
            var sourceImage = Image.FromStream(stream);
            Byte[] data;
            
     
            using (Bitmap bmPhoto = new Bitmap(width, height))
            {
                bmPhoto.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);


                using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                {
                    grPhoto.Clear(System.Drawing.Color.White);
                    grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                    grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;


                    grPhoto.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, width, height)
                        , new System.Drawing.Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
                }


                ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[1];
                EncoderParameters eParams = new EncoderParameters(1);
                eParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                
                using (var memoryStream = new MemoryStream())
                {
                    bmPhoto.Save(memoryStream, codec, eParams);

                    data = memoryStream.ToArray();
                }
                eParams.Dispose();
            }
           


            return data;
            
          
            
        }
        
        public static byte[] Resize(byte[] imageData, int width, int height)
        {
            var stream = new MemoryStream(imageData);
            var image = Image.FromStream(stream);
            var destRect = new Rectangle(0, 0, 1366, 768);
            var destImage = new Bitmap(width, height);
            
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            Byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                destImage.Save(memoryStream, ImageFormat.Bmp);

                data = memoryStream.ToArray();
            }
            return data;
            
        }
        */
        private byte[] resizingImage(byte[] imageData, int size = 500, int h = 0)
        {
            // Si size es 0, retornar la imagen original sin redimensionar
            if (size == 0)
            {
                return imageData;
            }

            using (var ms = new MemoryStream(imageData))
            {
                using (var image = Image.FromStream(ms))
                {
                    int width, height;

                    // Calcular dimensiones
                    if (size > 0 && h > 0)
                    {
                        width = size;
                        height = h;
                    }
                    else
                    {
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }

                    // Solo redimensionar si la imagen es más grande que el tamaño objetivo
                    // Esto evita agrandar imágenes pequeñas y pixelarlas
                    if (image.Width <= width && image.Height <= height)
                    {
                        return imageData; // Retornar imagen original sin cambios
                    }

                    using (var resized = new Bitmap(width, height))
                    {
                        resized.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                        using (var graphics = Graphics.FromImage(resized))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphics.DrawImage(image, 0, 0, width, height);
                        }

                        // Obtener el encoder correcto según el formato original
                        ImageFormat format = image.RawFormat;
                        ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()
                            .FirstOrDefault(c => c.FormatID == format.Guid);

                        // Si no se encuentra encoder, usar JPEG por defecto
                        if (codec == null)
                        {
                            codec = ImageCodecInfo.GetImageEncoders()
                                .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                            format = ImageFormat.Jpeg;
                        }

                        using (var imageNew = new MemoryStream())
                        {
                            // Configurar calidad (85 es un buen balance entre calidad y tamaño)
                            var encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 85L);

                            resized.Save(imageNew, codec, encoderParameters);
                            return imageNew.ToArray();
                        }
                    }
                }
            }
        }
    }
}


