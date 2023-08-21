using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetAllVoiceActors()
    {
        var voiceActors = await _voiceActorService.GetAllVoiceActors();
        var voiceActorResponseDTOs = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActors);
        return Ok(voiceActorResponseDTOs);
    }
    
    [HttpGet("{voiceActorId:guid}")]
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