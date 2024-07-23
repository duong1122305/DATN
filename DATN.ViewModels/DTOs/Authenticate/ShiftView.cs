using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class ShiftView
    {
        public string Name { get; set; }
        [Required, Range(1, 24, ErrorMessage = "Vui lòng nhập từ 1 đến 24")]

        public int To { get; set; }
        [Required, Range(1, 24, ErrorMessage = "Vui lòng nhập từ 1 đến 24")]

        public int From { get; set; }
    }
}
