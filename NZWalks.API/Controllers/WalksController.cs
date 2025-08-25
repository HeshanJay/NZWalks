using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            // Map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        // GET Walks
        // GET: /api/walks?filterOn=Name&filterQuery="Track"&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

            // Map domain model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }


        // GET Walk by ID
        // GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
                return NotFound();

            // Map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        // UPDATE Walk by ID
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidationModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
                return NotFound();

            // Map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        // DELETE a Walk by ID
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
                return NotFound();

            // Map domain model to DTO 
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
