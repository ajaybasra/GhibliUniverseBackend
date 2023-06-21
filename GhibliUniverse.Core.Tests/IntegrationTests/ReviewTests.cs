using AutoMapper;
using GhibliUniverse.API.Mapper;

namespace GhibliUniverse.Core.Tests.IntegrationTests;

public class ReviewTests
{
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseWebApplicationFactory<Program> _ghibliUniverseWebApplicationFactory;
    private readonly HttpClient _client;

    public ReviewTests()
    {
        _mappingProfiles = new MappingProfiles();
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        _mapper = new Mapper(_mapperConfiguration);
        _ghibliUniverseWebApplicationFactory = new GhibliUniverseWebApplicationFactory<Program>();
        _client = _ghibliUniverseWebApplicationFactory.CreateClient();
    }
}