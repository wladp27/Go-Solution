
using System.ComponentModel.DataAnnotations;


namespace GoWeb.Shared.Models
{
    public class EventFilterDTO 
    {
        [Required(ErrorMessage = "Пожалуйста,выберите город")]
        [Range(1, int.MaxValue)]
        [Display(Name = "Город")]
        public int? SelectedCity { get; set; }
        
        public double[]? SelectedCityCoordinate { get; set; }

        public List<CityDTO>? Cities{ get; set; }

        [Display(Name = "Тип события")]
        public int? SelectedTypeEvent { get; set; }

        public List<EventTypeDTO>? TypeEvents { get; set; }



        public override bool Equals(object? obj)
        {
            return Equals(obj as EventFilterDTO);
        }

        public virtual bool Equals(EventFilterDTO? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return SelectedCity == other.SelectedCity &&
                   SelectedTypeEvent == other.SelectedTypeEvent;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(SelectedCity, SelectedTypeEvent);
        }

    }
}
