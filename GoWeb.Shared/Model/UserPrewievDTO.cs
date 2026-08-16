

namespace GoWeb.Shared.Models
{
    public class UserPrewievDTO
    {
        public string Id { get; set; }
        public string UserName {  get; set; }
        public List<RatingDTO> Ratings { get; set; }
        public string DisplayName { get; set; }

    }
}
