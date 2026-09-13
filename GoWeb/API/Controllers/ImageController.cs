using GoWeb.Models;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace GoWeb.API.Controllers
{

    [ApiController]
    public class ImageController : ControllerBase
    {


        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost(AddImageEventRequest.RouteTemplate)]
        public async Task<ActionResult<AddImageEventRequest.Response>> WriterImages(IFormFile image)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "events");
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (image != null)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    return BadRequest(AddImageEventRequest.Response.Failure("Недопустимый формат изображения"));
                
                if (image.Length > 10 * 1024 * 1024) 
                    return BadRequest(AddImageEventRequest.Response.Failure("Размер изображения превышает допустимый лимит (10 MB)"));

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return Ok(AddImageEventRequest.Response.Success(uniqueFileName));
            }
            return BadRequest(AddImageEventRequest.Response.Failure("Ошибка загрузки изображения"));
        }


        [HttpDelete(DeleteImageEventRequest.RouteTemplate)]
        public async Task<ActionResult<DeleteImageEventRequest.Response>> DeleteImage(string nameImage)
        {
            if (string.IsNullOrWhiteSpace(nameImage))
            {
                return BadRequest(DeleteImageEventRequest.Response.Failure("Имя файла не указано"));
            }

            string safeFileName = Path.GetFileName(nameImage);
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "events");
            string filePath = Path.Combine(uploadsFolder, safeFileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(DeleteImageEventRequest.Response.Failure("Файл не найден"));
            }
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (IOException)
            {
                return StatusCode(500, DeleteImageEventRequest.Response.Failure($"Ошибка доступа к файлу"));
            }

            return Ok(DeleteImageEventRequest.Response.Success());
        }
    

    }
}
