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
public class VoiceActorController : Controller
{

    private readonly IVoiceActorService _voiceActorService;
    private readonly IMapper _mapper;

    public VoiceActorController(IVoiceActorService voiceActorService, IMapper mapper)
    {
        _voiceActorService = voiceActorService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get All Voice Actors", Description = "Get all voice actors.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all existing voice actors.")]
    public async Task<IActionResult> GetAllVoiceActors()
    {
        var voiceActors = await _voiceActorService.GetAllVoiceActors();
        var voiceActorResponseDTOs = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActors);
        return Ok(voiceActorResponseDTOs);
    }
    
    [HttpGet("{voiceActorId:guid}")]
    [SwaggerOperation(Summary = "Get Voice Actor", Description = "Get voice actor by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns voice actor matching specified id.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Voice actor with specified id cannot be found.")]
    public async Task<IActionResult> GetVoiceActorById(Guid voiceActorId)
    {
        try
        {
            var voiceActor = await _voiceActorService.GetVoiceActorById(voiceActorId);
            var voiceActorResponseDTO = _mapper.Map<VoiceActorResponseDTO>(voiceActor);
            return Ok(voiceActorResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
    }
    
    [HttpGet("{voiceActorId:guid}/films")]
    [SwaggerOperation(Summary = "Get Films by Voice Actor", Description = "Get films by voice actor.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all existing films for the voice actor id you specify.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Voice actor with specified id cannot be found.")]
    public async Task<IActionResult> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        try
        {
            var filmsByVoiceActor = await _voiceActorService.GetFilmsByVoiceActor(voiceActorId);
            var filmsByVoiceActorResponseDTO = _mapper.Map<List<VoiceActorFilmResponseDTO>>(filmsByVoiceActor);
            return Ok(filmsByVoiceActorResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Create Voice Actor", Description = "Create voice actor.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns newly created voice actor.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request was formatted incorrectly.")]
    public async Task<IActionResult> CreateVoiceActor([FromBody] VoiceActorRequestDTO voiceActorCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var voiceActorCreateRequestAsValueObject = _mapper.Map<VoiceActor>(voiceActorCreate);
            var createdVoiceActor = await _voiceActorService.CreateVoiceActor(voiceActorCreateRequestAsValueObject);
            var voiceActorResponseDTO = _mapper.Map<VoiceActorResponseDTO>(createdVoiceActor);
            return Ok(voiceActorResponseDTO);
        }
        catch (AutoMapperMappingException ex)
        {
            return BadRequest(ex.InnerException.Message);
        }
    }
    
    [HttpPut("{voiceActorId:guid}")]
    [SwaggerOperation(Summary = "Update Voice Actor", Description = "Update voice actor by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Voice actor updated successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Voice actor with specified id cannot be found.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request was formatted incorrectly.")]
    public async Task<IActionResult> UpdateVoiceActor(Guid voiceActorId, [FromBody] VoiceActorRequestDTO voiceActorUpdate)
    {
        try
        {
            var voiceActorUpdateRequestAsValueObject = _mapper.Map<VoiceActor>(voiceActorUpdate);
            var updatedVoiceActor = await _voiceActorService.UpdateVoiceActor(voiceActorId, voiceActorUpdateRequestAsValueObject);
            var voiceActorResponseDTO = _mapper.Map<VoiceActorResponseDTO>(updatedVoiceActor);
            return Ok(voiceActorResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
        catch (AutoMapperMappingException ex)
        {
            return BadRequest(ex.InnerException.Message);
        }
    }
    
    [HttpDelete("{voiceActorId:guid}")]
    [SwaggerOperation(Summary = "Delete Voice Actor", Description = "Delete voice actor by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Voice actor deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Voice actor with specified id cannot be found.")]
    public async Task<IActionResult> DeleteVoiceActor(Guid voiceActorId)
    {
        try
        {
            await _voiceActorService.DeleteVoiceActor(voiceActorId);
            return Ok("Successfully deleted voice actor");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
    }
}