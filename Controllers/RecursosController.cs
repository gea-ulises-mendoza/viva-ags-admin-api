using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using VivaAguascalientesAPI.DTO.RequestDTO;


namespace UbikAgsAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController, Produces("application/json")]
    [Authorize]
    public class RecursosController : Controller
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Video([FromBody] CreateVideoRequest request)
        {
            System.IO.File.WriteAllBytes("recursos/" + request.Nombre, request.Video);
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet("videoprincipal")]
        [Produces("application/image")]
        public ActionResult GetVideo()
        {
            var filename = "reel.mp4";
            //Build the File Path.
            string path = Path.Combine("recursos/") + filename;  // the video file is in the wwwroot/files folder
    
            var filestream = System.IO.File.OpenRead(path);
            return File(filestream, contentType: "video/mp4", fileDownloadName: filename, enableRangeProcessing: true); 
        }
        
        
        [HttpPost("SubeVideo")]
        [Produces("application/json")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            // Get the file from the POST request
           // var theFile = files[0];//HttpContext.Request.Form.Files.GetFile("file");

            // Get the server path, wwwroot
            string webRootPath = "";
  
            // Building the path to the uploads directory
            var fileRoute = Path.Combine(webRootPath, "recursos");
  
            // Get the mime type
            var mimeType = file.ContentType;
  
            // Get File Extension
            string extension = System.IO.Path.GetExtension(file.FileName);
  
            // Generate Random name.
            string name = "reel"+extension;//Guid.NewGuid().ToString().Substring(0, 8) + extension;
  
            // Build the full path inclunding the file name
            string link = Path.Combine(fileRoute, name);
  
            // Create directory if it dose not exist.
            FileInfo dir = new FileInfo(fileRoute);
            dir.Directory.Create();
  
            // Basic validation on mime types and file extension
            string[] videoMimetypes = { "video/mp4", "video/webm", "video/ogg" };
            string[] videoExt = { ".mp4", ".webm", ".ogg" };
  
            try
            {
                if (Array.IndexOf(videoMimetypes, mimeType) >= 0 && (Array.IndexOf(videoExt, extension) >= 0))
                {
                    // Copy contents to memory stream.
                    Stream stream;
                    stream = new MemoryStream();
                    file.CopyTo(stream);
                    stream.Position = 0;
                    String serverPath = link;
  
                    // Save the file
                    using (FileStream writerFileStream = System.IO.File.Create(serverPath))
                    {
                        await stream.CopyToAsync(writerFileStream);
                        writerFileStream.Dispose();
                    }
  
                    // Return the file path as json
                    Hashtable videoUrl = new Hashtable();
                    videoUrl.Add("link", "/recursos/" + name);
  
                    return Json(videoUrl);
                }
                throw new ArgumentException("The video did not pass the validation");
            }
  
            catch (ArgumentException ex)
            {
                return Json(ex.Message);
            }
        }
    }
}