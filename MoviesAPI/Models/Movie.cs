using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(250, ErrorMessage = "Title cannot exceed 250 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Year is required.")]
        [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100.")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Rate is required.")]
        [Range(0, 10, ErrorMessage = "Rate must be between 0 and 10.")]
        public double Rate { get; set; }
        [Required(ErrorMessage = "StoreLine is required.")]
        [MaxLength(2500, ErrorMessage = "StoreLine cannot exceed 2500 characters.")]
        public string StoreLine { get; set; }
        
        [Required(ErrorMessage = "GenreId is required.")]
        public byte GenreId { get; set; }
       
    }
}
