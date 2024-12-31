using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class genresController : ControllerBase
    {
        MoviesContext context;
        public genresController(MoviesContext _context)
        {
            context = _context;
        }

        [HttpGet("with-movies")]
        public IActionResult GetAllGenresWithMovies()
        {
            var genresWithMovies = context.Genres
                                          //.Include(g => g.Movies)
                                          .ToList();
            return Ok(genresWithMovies);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            Genre genre =
               context.Genres.FirstOrDefault(d=>d.Id==id);
            return Ok(genre);
        }
        [HttpPost]
        public IActionResult CreateGenre(Genre genre) 
        {
       context.Genres.Add(genre);
            context.SaveChanges();
            //return CreatedAtAction("GetById", genre);
            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre); 

        }
        [HttpPut("{id}")]
        public IActionResult UpdateGenre(int id,  Genre updateGenre)
        {
           
            
           var genre= context.Genres.FirstOrDefault(d=>d.Id==id);
            if(genre == null)
            
                return NotFound("Genre Not Found");
            
            genre.Name = updateGenre.Name;
            context.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteById(int id) 
        {
            var genre = context.Genres.FirstOrDefault(d => d.Id == id);
            if (genre == null)
                return NotFound("Genre Not Found");
            context.Genres.Remove(genre);
            context.SaveChanges();
            return NoContent();


        }
        [HttpGet("search/{name}")]
        public IActionResult SearchByName(string name)
        {
            var genres = context.Genres
                                .Where(g => g.Name.Contains(name))
                                .ToList();
            if (!genres.Any())
                return NotFound("No genres found with the provided name.");
            return Ok(genres);
        }
        [HttpGet("{id}/movies")]
        public IActionResult GetMoviesByGenreId(int id)
        {
            var genre = context.Genres
                               //.Include(g => g.Movies)
                               .FirstOrDefault(g => g.Id == id);

            if (genre == null)
                return NotFound("Genre not found.");

            return Ok(genre);
        }
        [HttpHead("{id}")]
        public IActionResult CheckIfGenreExists(int id)
        {
            bool exists = context.Genres.Any(g => g.Id == id);
            return exists ? Ok() : NotFound("Genre not found.");
        }
        [HttpDelete("reset")]
        public IActionResult ResetGenres()
        {
            var allGenres = context.Genres.ToList();
            if (!allGenres.Any())
                return NotFound("No genres to delete.");

            context.Genres.RemoveRange(allGenres);
            context.SaveChanges();
            return NoContent();
        }

    }
}
