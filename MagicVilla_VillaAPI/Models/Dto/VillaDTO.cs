using System;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
	public class VillaDTO
	{
        /*Why DTO?
         * in production application, we do not work with the actual model rather use a DTO(Data Transfer Object) model
         * Separate the service layer from the database layer
         * Hide specific properties that clients don’t need to receive
         * DTOs provide an efficient way to separate domain objects from the presentation layer. This way, you can change the presentation layer without affecting the existing domain layers, and vice versa.
         * 
         * For example: in the actual model: we have CreatedDate in our actual model (Villa.cs) but we do not want to share this data with the user, so it is not available in the DTO.
         * **/
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}

