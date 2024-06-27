using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    [Table("ImagePets")]
    public class ImagePet
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
        public int PetId { get; set; }

        public Pet Pet { get; set; }
    }
}
