using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoiceActorController : Controller
{
    [HttpGet]
    public IActionResult GetAllVoiceActors()
    {
        return Ok("ye");
    }
    
    [HttpGet("{voiceActorId:guid}")]
    public IActionResult GetVoiceActorById(Guid voiceActorId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{voiceActorId:guid}/films")]
    public IActionResult GetFilmsByVoiceActor(Guid voiceActorId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost]
    public IActionResult CreateVoiceActor([FromBody]ValidatedString name)
    {
        return Ok("yo");
    }
    
    [HttpPut("{voiceActorId:guid}")]
    public IActionResult UpdateVoiceActor(Guid voiceActorId)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{voiceActorId:guid}")]
    public IActionResult DeleteVoiceActor(Guid voiceActorId)
    {
        throw new NotImplementedException();
    }
}