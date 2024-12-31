using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Genre name is required.")]
        [MaxLength(50, ErrorMessage = "Genre name cannot exceed 50 characters.")]
        public string Name { get; set; }
       
    }
}
