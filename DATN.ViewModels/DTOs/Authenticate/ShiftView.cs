using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class ShiftView
    {
        public string Name { get; set; }
        [Required]
        public int To { get; set; }
        [Required]
        public int From { get; set; }
    }
}
