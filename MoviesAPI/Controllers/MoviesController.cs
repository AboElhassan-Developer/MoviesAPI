using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        MoviesContext context;
        public MoviesController(MoviesContext _context)
        {
            context = _context;
        }
        //[HttpGet]
        //public IActionResult GetAllMovies()
        //{
        //    //List<Movie> movies =
        //    //    context.Movies.ToList();
        //    //return Ok(movies);
        //   // استخدام Include لتحميل البيانات المرتبطة بـ Genre مع الأفلام
        //   var moviesWithGenres = context.Movies.Include(m => m.Genre).ToList();
        //    return Ok(moviesWithGenres);
        //}
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var movies = context.Movies
                //.Include(m => m.Genre)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Year,
                    m.Rate,
                    m.StoreLine,
                    m.GenreId,
                  /*  GenreName = m.Genre.Name*/ // إعادة اسم النوع فقط إذا لزم الأمر
                })
                .ToList();

            return Ok(movies);
        }
        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = context.Movies/*.Include(m => m.Genre)*/.FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var movieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rate = movie.Rate,
                StoreLine = movie.StoreLine,
               /* Poster = Convert.ToBase64String(movie.Poster),*/ // إذا كانت الصورة مخزنة كـ byte[]
                //GenreName = movie.Genre?.Name
            };

            return Ok(movieViewModel);
        }

        [HttpPost]
        public IActionResult CreateMovie([FromBody]Movie movie)
        {

            if(!ModelState .IsValid)
            {
                return BadRequest(ModelState);
            }


            context.Movies.Add(movie);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetMovieById),new { id = movie.Id },movie);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, Movie updateMovie)
        {
            if (id != updateMovie.Id)
            {
                return BadRequest("Id mismatch.");
            }
            var movie = context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null)
                return NotFound(movie);
            movie.Title = updateMovie.Title;
            movie.Year = updateMovie.Year;
            movie.Rate = updateMovie.Rate;
            movie.StoreLine = updateMovie.StoreLine;
            movie.GenreId = updateMovie.GenreId;
            context.SaveChanges();
            return NoContent();// Indicates success with no content returned.
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound("Movie not found.");

            context.Movies.Remove(movie);
            context.SaveChanges();
            return Ok("Movie deleted successfully.");
        }

        [HttpGet("searsh")]
        public IActionResult SearshMovie([FromQuery] string title)
        {
            var movie = context.Movies
                .Where(x => x.Title.Contains(title))
                .ToList();
            if (!movie.Any())
            {
                return NotFound("No movies match the search criteria.");
            }
            return Ok(movie);
        }


        [HttpGet("byGenre/{genreId}")]
        public IActionResult GetMoviesByGenre(int genreId)
        {
            var movies = context.Movies
                .Where(m => m.GenreId == genreId)
                .ToList();

            if (!movies.Any())
                return NotFound("No movies found for the specified genre.");

            return Ok(movies);
        }
     
        [HttpGet("averageRate")]
        public IActionResult GetAverageRate()
        {
            var averageRate=context.Movies.Average(x => x.Rate);
            return Ok(new {AveragRate=averageRate});
        }

    }
}
