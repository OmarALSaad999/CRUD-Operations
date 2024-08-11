using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Market.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [MaxLength(15)]
        public string Name { get; set; }

        [DisplayName(" DisplayOrder")]
        [Range(1, 100, ErrorMessage = " DisplayOrdermus be between 1-100")]
        public int DisplayOrder { get; set; }

        //

        [DisplayName("Image Path")]
        public string? ImageUrl { get; set; } // New property

    }

}

