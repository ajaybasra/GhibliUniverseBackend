using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
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
        var films = _mapper.Map<List<FilmResponseDTO>>(_filmService.GetAllFilms());
        return Ok(films);
    }

    [HttpGet("{filmId:guid}")]
    public IActionResult GetFilmById(Guid filmId)
    {
        try
        {
            var film = _mapper.Map<FilmResponseDTO>(_filmService.GetFilmById(filmId));
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
            var voiceActorsByFilm = _mapper.Map<List<VoiceActorResponseDTO>>(_filmService.GetVoiceActorsByFilm(filmId));
            return Ok(voiceActorsByFilm);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }
    
    [HttpPost]
    public IActionResult CreateFilm([FromBody] FilmRequestDTO filmCreate)
    {
        if (filmCreate == null)
        {
            return BadRequest(ModelState);
        }
        
        if (_filmService.FilmTitleAlreadyExists(filmCreate.Title))
        {
            ModelState.AddModelError("", "Film with the same name already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _filmService.CreateFilm(filmCreate.Title, filmCreate.Description, filmCreate.Director, filmCreate.Composer, filmCreate.ReleaseYear);
        
        return Ok("Successfully created film");
    }

    [HttpPost("{filmId:guid}/LinkVoiceActor")]
    public IActionResult LinkVoiceActor(Guid filmId, [FromBody] Guid voiceActorId)
    {
        try
        {
            _filmService.LinkVoiceActor(filmId, voiceActorId);
            return Ok("Successfully linked voice actor");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No model found with the given id");
        }
    }
    
    [HttpPost("{filmId:guid}/UnlinkVoiceActor")]
    public IActionResult UnlinkVoiceActor(Guid filmId, [FromBody] Guid voiceActorId)
    {
        try
        {
            _filmService.UnlinkVoiceActor(filmId, voiceActorId);
            return Ok("Successfully unlinked voice actor");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No model found with the given id");
        }
    }

    [HttpPut("{filmId:guid}")]
    public IActionResult UpdateFilm(Guid filmId, [FromBody] FilmRequestDTO filmUpdate)
    {
        if (filmUpdate == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var filmUpdateToValueObjects = new Film()
            {
                Title = ValidatedString.From(filmUpdate.Title), // worth looking into configuring automapper to map 
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