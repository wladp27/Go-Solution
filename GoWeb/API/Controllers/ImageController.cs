using GoWeb.Models;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

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
    }
}
