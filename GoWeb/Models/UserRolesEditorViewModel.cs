
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoWeb.Models
{
    public class UserRolesEditorViewModel 
    {
       public string UserName { get; set; }
       public string DisplayName { get; set; }
       public IList<string>? RolesUser {  get; set; }
       public IEnumerable<RoleView>? AllRoles { get; set; }

       [DisplayName("Добавить роль")]
       [Required(ErrorMessage = "Пожалуйста выберите роль")]
       public string SelectedRole { get; set; }
    }
}
