using AutoMapper;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;

[ApiController]
[Route("api/[controller]")]  
public class FilmController : Controller
{
    private readonly IFilmService _filmService;
    private readonly IMapper _mapper;
    
    public FilmController(IFilmService filmService, IMapper mapper)
    {
        _filmService = filmService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAllFilms()
    {
        var films = _filmService.GetAllFilms();
        return Ok(films);
    }

    [HttpGet("{filmId:guid}")]
    public IActionResult GetFilmById(Guid filmId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{filmId:guid}/voiceActors")]
    public IActionResult GetVoiceActorsByFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }
// https://localhost:7212/api/film
    [HttpPost]
    public IActionResult CreateFilm([FromBody]Film film)
    {
        return Ok("yo");
    }

    [HttpPut("{filmId:guid}")]
    public IActionResult UpdateFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{filmId:guid}")]
    public IActionResult DeleteFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }
    
    
}