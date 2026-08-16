using System.ComponentModel.DataAnnotations;
using GoWeb.Shared.Models;
namespace GoWeb.Models
{
    public class LocationCreateViewModel: LocationDTO
    {
       

        [Display(Name = "Фото локации")]
        public List<IFormFile>? imagesLocation { get; set; }

    
    }
}
