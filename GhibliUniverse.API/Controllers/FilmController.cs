using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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
        var films = _mapper.Map<List<FilmDTO>>(_filmService.GetAllFilms());
        return Ok(films);
    }

    [HttpGet("{filmId:guid}")]
    public IActionResult GetFilmById(Guid filmId)
    {
        try
        {
            var film = _mapper.Map<FilmDTO>(_filmService.GetFilmById(filmId));
            return Ok(film);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }

    [HttpGet("{filmId:guid}/voiceActors")]
    public IActionResult GetVoiceActorsByFilm(Guid filmId)
    {
        try
        {
            var voiceActorsByFilm = _mapper.Map<List<VoiceActorDTO>>(_filmService.GetVoiceActorsByFilm(filmId));
            return Ok(voiceActorsByFilm);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }
// https://localhost:7212/api/film
    [HttpPost]
    public IActionResult CreateFilm([FromBody] FilmPostDTO filmCreate)
    {
        if (filmCreate == null)
        {
            return BadRequest(ModelState);
        }

        var film = _filmService.GetAllFilms().FirstOrDefault(f => f.Title == ValidatedString.From(filmCreate.Title));

        if (film != null)
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _filmService.CreateFilm(filmCreate.Title, filmCreate.Description, filmCreate.Director, filmCreate.Composer, filmCreate.ReleaseYear);
        
        return Ok("Successfully created film");
    }


    [HttpPut("{filmId:guid}")]
    public IActionResult UpdateFilm(Guid filmId, [FromBody] FilmPostDTO filmUpdate)
    {
        if (filmUpdate == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var film = _filmService.GetFilmById(filmId);
            var filmUpdateToValueObjects = new Film()
            {
                Title = ValidatedString.From(filmUpdate.Title),
                Description = ValidatedString.From(filmUpdate.Description),
                Director = ValidatedString.From(filmUpdate.Director),
                Composer = ValidatedString.From(filmUpdate.Composer),
                ReleaseYear = ReleaseYear.From(filmUpdate.ReleaseYear)
            };
            _filmService.UpdateFilm(filmId, filmUpdateToValueObjects);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
        
        return Ok("Successfully updated film");
    }

    [HttpDelete("{filmId:guid}")]
    public IActionResult DeleteFilm(Guid filmId)
    {
        try
        {
            _filmService.DeleteFilm(filmId);
            return Ok("Successfully deleted film");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }
    
    
}