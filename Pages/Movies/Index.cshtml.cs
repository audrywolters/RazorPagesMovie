using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> MovieList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public SelectList Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }


        public async Task OnGetAsync()
        {
            // get all the movies
            var movies = from m in _context.Movie 
                         select m;

            // get all the genres
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            // put genres into a select list for user to pick from
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

            
            if (!string.IsNullOrEmpty(SearchString))
            {
                // filter by what title user typed
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }


            if (!string.IsNullOrEmpty(MovieGenre))
            {
                // filter list by genre
                movies = movies.Where(x => x.Genre == MovieGenre);
            }

            // return filtered movies
            MovieList = await movies.ToListAsync();
        }
    }
}
