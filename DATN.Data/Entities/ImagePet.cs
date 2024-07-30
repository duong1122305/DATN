using System.ComponentModel.DataAnnotations.Schema;

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
