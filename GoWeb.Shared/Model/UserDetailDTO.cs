using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Model
{
    public class UserDetailDTO
    {
        public string DisplayName { get; set; }
        public double ReliabilityVisit { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int idCity { get; set; }
    }
}
