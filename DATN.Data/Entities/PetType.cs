using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    [Table("PetType")]
	public class PetType
	{
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
