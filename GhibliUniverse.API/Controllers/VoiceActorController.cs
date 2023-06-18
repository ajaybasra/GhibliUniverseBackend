using AutoMapper;
using GhibliUniverse.API.DTOs;
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
    public IActionResult GetAllVoiceActors()
    {
        var voiceActors = _mapper.Map<List<VoiceActorResponseDTO>>(_voiceActorService.GetAllVoiceActors());
        return Ok(voiceActors);
    }
    
    [HttpGet("{voiceActorId:guid}")]
    public IActionResult GetVoiceActorById(Guid voiceActorId)
    {
        try
        {
            var voiceActor = _mapper.Map<VoiceActorResponseDTO>(_voiceActorService.GetVoiceActorById(voiceActorId));
            return Ok(voiceActor);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
    }
    
    [HttpGet("{voiceActorId:guid}/films")]
    public IActionResult GetFilmsByVoiceActor(Guid voiceActorId)
    {
        try
        {
            var filmsByVoiceActor = _mapper.Map<List<FilmResponseDTO>>(_voiceActorService.GetFilmsByVoiceActor(voiceActorId));
            return Ok(filmsByVoiceActor);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
    }
    
    [HttpPost]
    public IActionResult CreateVoiceActor([FromBody] VoiceActorRequestDTO voiceActorCreate)
    {
        if (voiceActorCreate == null)
        {
            return BadRequest(ModelState);
        }
        
        // if (_voiceActorService.VoiceActorAlreadyExists(voiceActorCreate.Name))
        // {
        //     ModelState.AddModelError("", "Voice actor with the same name already exists");
        //     return StatusCode(422, ModelState);
        // }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdVoiceActor = _mapper.Map<VoiceActorResponseDTO>(_voiceActorService.CreateVoiceActor(voiceActorCreate.Name));
            return Ok(createdVoiceActor);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpPut("{voiceActorId:guid}")]
    public IActionResult UpdateVoiceActor(Guid voiceActorId, [FromBody] VoiceActorRequestDTO voiceActorUpdate)
    {
        if (voiceActorUpdate == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedVoiceActor = _mapper.Map<VoiceActorResponseDTO>(_voiceActorService.UpdateVoiceActor(voiceActorId, voiceActorUpdate.Name));
            return Ok(updatedVoiceActor);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        
        
    }
    
    [HttpDelete("{voiceActorId:guid}")]
    public IActionResult DeleteVoiceActor(Guid voiceActorId)
    {
        try
        {
            _voiceActorService.DeleteVoiceActor(voiceActorId);
            return Ok("Successfully deleted voice actor");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No voice actor found with the following id: " + voiceActorId);
        }
    }
}