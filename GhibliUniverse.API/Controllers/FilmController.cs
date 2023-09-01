using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Get All Films", Description = "Get all films.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all existing films.")]
    public async Task<IActionResult> GetAllFilms()
    {
        var films = await _filmService.GetAllFilms();
        var filmResponseDTOs = _mapper.Map<List<FilmResponseDTO>>(films);
        return Ok(filmResponseDTOs);
    }
    
    [HttpGet("{filmId:guid}")]
    [SwaggerOperation(Summary = "Get Film", Description = "Get film by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns film matching specified id.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Film with specified id cannot be found.")]
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
    [SwaggerOperation(Summary = "Get Voice Actors by Film", Description = "Get voice actors by film.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all existing voice actors for the film id you specify.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Film with specified id cannot be found.")]
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
    [SwaggerOperation(Summary = "Create Film", Description = "Create film.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns newly created film.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request was formatted incorrectly.")]
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
    [SwaggerOperation(Summary = "Link Voice Actor", Description = "Link voice actor.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns success message.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Model with specified id cannot be found.")]
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
    [SwaggerOperation(Summary = "Unlink Voice Actor", Description = "Unlink voice actor.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns success message.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Model with specified id cannot be found.")]
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
    [SwaggerOperation(Summary = "Update Film", Description = "Update film by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Film updated successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Film with specified id cannot be found.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request was formatted incorrectly.")]
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
    [SwaggerOperation(Summary = "Delete Film", Description = "Delete film by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Film deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Film with specified id cannot be found.")]
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