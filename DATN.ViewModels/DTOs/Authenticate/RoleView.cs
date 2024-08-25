using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class RoleView
    {
        public Guid IdRole { get; set; }
        
        public string NameRole { get; set; }
    }
}
