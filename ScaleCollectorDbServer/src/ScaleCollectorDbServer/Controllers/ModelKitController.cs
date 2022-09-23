using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScaleCollectorDbServer.Contracts.v1.Requests;
using ScaleCollectorDbServer.Contracts.v1.Responses;
using ScaleCollectorDbServer.Data;
using ScaleCollectorDbServer.Data.Entities;

namespace ScaleCollectorDbServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelKitsController : ControllerBase
    {
        private readonly ILogger<ModelKitsController> _logger;
        private readonly ExtendedScaleDbContext _dbContext;
        private readonly IMapper _mapper;

        public ModelKitsController(ILogger<ModelKitsController> logger, ExtendedScaleDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ModelKitResponse>> GetAll()
        {
            var x = _mapper.ProjectTo<ModelKitResponse>(_dbContext._context.ModelKits.AsNoTracking(), null).AsEnumerable();

            return Ok(x);
        }

        [HttpPut]
        public async Task<ActionResult<ModelKitResponse>> PutAsync(ModelKitPutRequest modelKitPutRequest)
        {
            var model = _mapper.Map<ModelKitPutRequest, ModelKit>(modelKitPutRequest);
            var updated = _dbContext._context.ModelKits.Find(model.Id);

            if (updated == null)
            {
                return NotFound(model.Id);
            }

            _dbContext._context.Entry(updated).CurrentValues.SetValues(model);

            _ = await _dbContext._context.SaveChangesAsync();

            return Ok(_mapper.Map<ModelKit, ModelKitResponse>(updated));
        }

        [HttpPost]
        public async Task<ActionResult<ModelKitResponse>> PostAsync(ModelKitPostRequest modelKitPostRequest)
        {
            var model = _mapper.Map<ModelKitPostRequest, ModelKit>(modelKitPostRequest);
            var inserted = _dbContext._context.Add(model);

            _ = await _dbContext._context.SaveChangesAsync();

            return Ok(_mapper.Map<ModelKit, ModelKitResponse>(inserted.Entity));
        }
    }
}
