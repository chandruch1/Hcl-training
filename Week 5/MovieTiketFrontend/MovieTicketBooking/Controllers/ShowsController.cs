using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;
        private readonly ILogger<ShowsController> _logger;

        public ShowsController(IShowService showService, ILogger<ShowsController> logger)
        {
            _showService = showService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShowResponseDto>>> GetAll()
        {
            var shows = await _showService.GetAllShowsAsync();
            return Ok(shows);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ShowResponseDto>> GetById(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);
            if (show is null)
                return NotFound(new { message = $"Show with ID {id} not found." });

            return Ok(show);
        }

        [HttpGet("{id:int}/seats")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShowSeatDetailDto>>> GetSeats(int id)
        {
            var seats = await _showService.GetShowSeatsAsync(id);
            return Ok(seats);
        }

        [HttpGet("movie/{movieId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShowResponseDto>>> GetByMovie(int movieId)
        {
            var shows = await _showService.GetShowsByMovieAsync(movieId);
            return Ok(shows);
        }

        [HttpGet("theatre/{theatreId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShowResponseDto>>> GetByTheatre(int theatreId)
        {
            var shows = await _showService.GetShowsByTheatreAsync(theatreId);
            return Ok(shows);
        }

        [HttpGet("date/{date:datetime}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShowResponseDto>>> GetByDate(DateTime date)
        {
            var shows = await _showService.GetShowsByDateAsync(date);
            return Ok(shows);
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<ActionResult<ShowResponseDto>> Create([FromBody] CreateShowDto dto)
        {
            var created = await _showService.CreateShowAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ShowId }, created);
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ShowResponseDto>> Update(int id, [FromBody] UpdateShowDto dto)
        {
            var updated = await _showService.UpdateShowAsync(id, dto);
            return Ok(updated);
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _showService.DeleteShowAsync(id);
            return NoContent();
        }
    }
}

