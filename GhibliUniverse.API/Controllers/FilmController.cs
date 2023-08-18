using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;
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
    public async Task<IActionResult> GetAllFilms()
    {
        var films = await _filmService.GetAllFilms();
        var filmResponseDTOs = _mapper.Map<List<FilmResponseDTO>>(films);
        return Ok(filmResponseDTOs);
    }
    
    [HttpGet("{filmId:guid}")]
    public async Task<IActionResult> GetFilmById(Guid filmId)
    {
        try
        {
            var film = await _filmService.GetFilmById(filmId);
            var filmResponseDTO = _mapper.Map<FilmResponseDTO>(film);
            return Ok(filmResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }
    
    [HttpGet("{filmId:guid}/voiceActors")]
    public async Task<IActionResult> GetVoiceActorsByFilm(Guid filmId)
    {
        try
        {
            var voiceActorsByFilm = await _filmService.GetVoiceActorsByFilm(filmId);
            var voiceActorDTOs = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActorsByFilm);
            return Ok(voiceActorDTOs);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateFilm([FromBody] FilmRequestDTO filmCreate)
    {
        if (await _filmService.FilmTitleAlreadyExists(filmCreate.Title)) // migrate
        {
            ModelState.AddModelError("", "Film with the same name already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var filmCreateRequestAsValueObject = _mapper.Map<Film>(filmCreate);
            var createdFilm = await _filmService.CreateFilm(filmCreateRequestAsValueObject);
            var filmResponseDTO = _mapper.Map<FilmResponseDTO>(createdFilm);
            return Ok(filmResponseDTO);
        }
        catch (AutoMapperMappingException ex)
        {
            return BadRequest(ex.InnerException.Message);
        }
        
    }
    
    [HttpPost("{filmId:guid}/LinkVoiceActor")]
    public async Task<IActionResult> LinkVoiceActor(Guid filmId, [FromBody] Guid voiceActorId)
    {
        try
        {
            await _filmService.LinkVoiceActor(filmId, voiceActorId);
            return Ok("Successfully linked voice actor");
        }
        catch (ModelNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("{filmId:guid}/UnlinkVoiceActor")]
    public async Task<IActionResult> UnlinkVoiceActor(Guid filmId, [FromBody] Guid voiceActorId)
    {
        try
        {
            await _filmService.UnlinkVoiceActor(filmId, voiceActorId);
            return Ok("Successfully unlinked voice actor");
        }
        catch (ModelNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPut("{filmId:guid}")]
    public async Task<IActionResult> UpdateFilm(Guid filmId, [FromBody] FilmRequestDTO filmUpdate)
    {
        try
        {
            var filmUpdateRequestAsValueObject = _mapper.Map<Film>(filmUpdate);
            var updatedFilm = await _filmService.UpdateFilm(filmId, filmUpdateRequestAsValueObject);
            var filmResponseDTO = _mapper.Map<FilmResponseDTO>(updatedFilm);
            return Ok(filmResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
        catch (AutoMapperMappingException e)
        {
            return BadRequest(e.InnerException.Message);
        }
    }

    [HttpDelete("{filmId:guid}")]
    public async Task<IActionResult> DeleteFilm(Guid filmId)
    {
        try
        {
            await _filmService.DeleteFilm(filmId);
            return Ok("Successfully deleted film");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No film found with the following id: " + filmId);
        }
    }

}