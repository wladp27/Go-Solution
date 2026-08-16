using System.ComponentModel.DataAnnotations;
using GoWeb.Shared.Models;
namespace GoWeb.Shared.Models
{
    public class EventSummaryDTO
    {

        public int Id { get; set; }

        [Display(Name = "Название события")]
        public string EventName { get; set; }

 

        [Display(Name = "Описание события")]
        public string? EventDescription { get; set; }

        public int EventTypeId { get; set; }

        [Display(Name = "Начало")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.Now.AddHours(1);

        [Display(Name = "Окончание")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTimeOffset EndTime { get; set; } = DateTimeOffset.Now.AddHours(2);

        [Display(Name = "Минимальное количество участников")]
        public int MinParticipants { get; set; }

        [Display(Name = "Максимальное количество участников")]
        public int MaxParticipants { get; set; }

        [Display(Name = " Стоимость участия в событии")]
        public decimal Price { get; set; }

        [Display(Name = "Организатор события")]
        public string? OrganizerName{ get; set; } = string.Empty;

        public string? OrganizerId { get; set; } = string.Empty;

        public int? StatusEventId { get; set; }
        public int? CountDaysRecreate { get; set; }
        public  int RequiredRating { get; set; } 

        public LocationDTO Location { get; set; }
        public string ImagePath { get; set; }
        public int CountRegisteredUsers { get; set; }



    }
}
